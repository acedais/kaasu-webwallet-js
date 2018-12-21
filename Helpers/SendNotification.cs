using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebWallet.Models;

namespace WebWallet.Helpers
{
    public static class SendNotification
    {

        private static ILogger logger = StaticLogger.CreateLogger("SendNotification");

        private static void LogException(Exception ex)
        {
            logger.Log(LogLevel.Error, ex.Message);
            logger.Log(LogLevel.Error, ex.StackTrace);
            if (ex.InnerException != null)
            {
                logger.Log(LogLevel.Error, string.Concat("Inner: ", ex.InnerException.Message));
                logger.Log(LogLevel.Error, ex.InnerException.StackTrace);
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(30)]
        public static void Send(PerformContext context)
        {
            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "txHash.db")))
                {
                    var CLHash = db.GetCollection<TxHash>("hash");
                    CLHash.EnsureIndex(x => x.hash);

                    // Get all hash pendding
                    List<TxHash> allHashs = CLHash.FindAll().Distinct().ToList();

                    foreach (var HashDetails in allHashs)
                    {
                        // Check hash + blockheight
                        logger.Log(LogLevel.Information, $"Check hash {HashDetails.hash} + publicKey {HashDetails.publicKey}");
                        try
                        {
                            var txHashes = new List<string>();
                            txHashes.Add(HashDetails.hash);

                            var tx_args = new Dictionary<string, object>();
                            tx_args.Add("transactionHashes", txHashes.ToArray());

                            var txs = RpcHelper.Request<TxDetailResp>("get_transaction_details_by_hashes", tx_args);

                            if (txs.transactions.Any() && txs.transactions[0].blockIndex > 0)
                            {
                                // Send notification
                                var response = SendNotificationHelper.SendFirebase<FirebaseMessage>(HashDetails);

                                

                                // Log response
                                if (response.success == 1)
                                {
                                    // Delete hash if send notification success
                                    CLHash.Delete(HashDetails.Id);
                                    logger.Log(LogLevel.Information, $"Send Notification Success {HashDetails.hash} + publicKey {HashDetails.publicKey}");

                                }
                                else if (response.failure == 1)
                                {
                                    CLHash.Delete(HashDetails.Id);
                                    logger.LogError($"Send Notification Error {HashDetails.hash} + publicKey {HashDetails.publicKey}");
                                }
                            }
                            else
                            {
                                logger.LogWarning($"Your transaction has not been confirmed: {HashDetails.hash}");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"Send Notification Error ");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                //finally, schedule the next check in 30 seconds time
                BackgroundJob.Schedule(() => SendNotification.Send(null), TimeSpan.FromSeconds(30));
            }
        }

        private static CachedTx AddSingleTransaction(string hash)
        {
            var tx_hash = new Dictionary<string, object>();
            tx_hash.Add("transactionHashes", new string[] { hash });
            //now try add the individual hash
            try
            {
                var txs = RpcHelper.Request<TxDetailResp>("get_transaction_details_by_hashes", tx_hash);
                var transactionsToInsert = new List<CachedTx>();
                foreach (var tx in txs.transactions)
                {
                    var cachedTx = TransactionHelpers.MapTx(tx);
                    return cachedTx;
                }
            }
            catch (Exception innerex)
            {
                //FAILED
                logger.LogError($"Failed to add hash: {hash}");
                LogException(innerex);
            }
            return null;
        }
    }
}
