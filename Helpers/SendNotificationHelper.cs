using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebWallet.Models;
using LiteDB;

namespace WebWallet.Helpers
{
    public static class SendNotificationHelper
    {
        public static T SendFirebase<T>(TxHash HashDetails)
        {
            // Check publicKey and send notification

            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "googleId.db")))
                {
                    var CLGoogleId = db.GetCollection<GoogleId>("googleid");
                    var resultGoogleId = CLGoogleId.Find(Query.EQ("publicKey", HashDetails.publicKey)).Distinct().ToList();
                    if (resultGoogleId.Any() && resultGoogleId[0].googleId != null)
                    {
                        try
                        {
                            using (WalletClient client = new WalletClient())
                            {
                                var notification = new
                                {
                                    to = resultGoogleId[0].googleId, //"f7ZG8fwG2dc:APA91bGIL9UJ3HHc_E9EEuLDhr4Id0lPylGgn0DHLKGOxfgVGlnN9WSfDITrJ_WOaNwh53gupceEU-dMp4CqmDuPcv5d9qD6BvzAVrkf2UTtc9Iszvqn96Zoqa00Ugr6owrEMIacy5cp",
                                    notification = new
                                    {
                                        body = "You just received " + ((decimal)Convert.ToInt32(HashDetails.amount) / 100000).ToString() + " Kaasu",
                                        title = "Congratulations"
                                    }
                                };
                                var dataString = JsonConvert.SerializeObject(notification);

                                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                                client.Headers[HttpRequestHeader.Authorization] = "key=" + SettingsProvider.CloudMessaging;

                                string response = client.UploadString("https://fcm.googleapis.com/fcm/send", dataString);
                                return JsonConvert.DeserializeObject<T>(response);
                            }
                        }
                        catch (Exception ex)
                        {
                            return default(T);
                        }
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                throw ex;
            }
        }
    }
}
