using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebWallet.Helpers;
using WebWallet.Models;


namespace WebWallet.Controllers
{
    [Route("api/[controller]/[action]")]
    public class NotificationController : Controller
    {

        [HttpGet("{publicKey}")]
        public Boolean checkGoogleId(string publicKey = "")
        {

            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "googleId.db")))
                {
                    var CLGoogleId = db.GetCollection<GoogleId>("googleid");
                    if (CLGoogleId.Exists(Query.EQ("publicKey", publicKey)))
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                return false;
                throw ex;
            }
        }

        [HttpGet("{googleId}")]
        public JsonResult FindGoogleId(string googleId = "")
        {

            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "googleId.db")))
                {
                    var CLGoogleId = db.GetCollection<GoogleId>("googleid");
                    var result = CLGoogleId.Find(x => x.googleId == googleId).Distinct();
                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                throw ex;
            }

            return new JsonResult("[]");
        }


        [HttpGet]
        public JsonResult FindAllGoogleId()
        {

            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "googleId.db")))
                {
                    var CLGoogleId = db.GetCollection<GoogleId>("googleid");
                    var result = CLGoogleId.FindAll().Distinct().ToList();

                    if (result.Any())
                    {
                        return new JsonResult(JsonConvert.SerializeObject(result));
                    }
                    else
                    {
                        return new JsonResult("[]");
                    }
                }
            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                throw ex;
            }

            
        }

        [HttpPost]
        public JsonResult SaveGoogleId([FromForm]GoogleId saveGoogleId)
        {
            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "googleId.db")))
                {
                    // Get a collection (or create, if doesn't exist)
                    var CLGoogleId = db.GetCollection<GoogleId>("googleid");

                    var result = CLGoogleId.Find(Query.EQ("googleId", saveGoogleId.googleId)).Distinct().ToList();

                    if (!result.Any())
                    {
                        // Insert new customer document (Id will be auto-incremented)
                        CLGoogleId.Insert(saveGoogleId);
                        // Index document using document Name property
                        CLGoogleId.EnsureIndex(x => x.publicKey);

                        return new JsonResult(JsonConvert.SerializeObject("Save Data Success"));
                    }
                    else if (result.Any())
                    {
                        // Update
                        saveGoogleId.Id = result[0].Id;
                        CLGoogleId.Update(saveGoogleId);
                        return new JsonResult(JsonConvert.SerializeObject("Update GoogleId Success"));
                    }
                }

            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                throw ex;
            }

            return new JsonResult("[]");
        }


        [HttpDelete("{googleId}")]
        public JsonResult DeleteGoogleId(string googleId = "")
        {
             try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "googleId.db")))
                {
                    // Get a collection (or create, if doesn't exist)
                    var CLGoogleId = db.GetCollection<GoogleId>("googleid");


                    //var result = googleid1.Find(x => x.googleId == googleId);


                    var result = CLGoogleId.Find(Query.EQ("googleId", googleId)).Distinct().ToList();
                    // Insert new customer document (Id will be auto-incremented)
                    if (result.Any()) { 
                        CLGoogleId.Delete(result[0].Id);
                        return new JsonResult(JsonConvert.SerializeObject("LogOut Success"));
                    }
                    else
                    {
                        return new JsonResult(JsonConvert.SerializeObject("LogOut Error"));
                    }
                }

            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                return new JsonResult("[]");
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult SaveTxHash([FromForm]TxHash saveTxHash)
        {
            try
            {
                var checkGGId = checkGoogleId(saveTxHash.publicKey);

                if (checkGGId) { 
                    using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "txHash.db")))
                    {
                        // Get a collection (or create, if doesn't exist)
                        var CLHash = db.GetCollection<TxHash>("hash");

                        if (!CLHash.Exists(Query.EQ("hash", saveTxHash.hash))) {

                            // Insert new customer document (Id will be auto-incremented)
                            CLHash.Insert(saveTxHash);

                            // Index document using document Name property
                            CLHash.EnsureIndex(x => x.hash);

                            return new JsonResult(JsonConvert.SerializeObject("Success"));
                        } else {

                             return new JsonResult(JsonConvert.SerializeObject("Error"));
                        }
                    }
                }
                else
                {
                    return new JsonResult(JsonConvert.SerializeObject("Miss GoogleId"));
                }

            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                return new JsonResult("[]");
                throw ex;
            }
        }


        [HttpGet("{hash}")]
        public JsonResult FindTxHash(string hash = "")
        {

            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "txHash.db")))
                {
                    var CLHash = db.GetCollection<TxHash>("hash");
                    var result = CLHash.Find(x => x.hash == hash).Distinct().ToList();
                    
                    return new JsonResult(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                throw ex;
            }

            return new JsonResult("[]");
        }


        [HttpGet]
        public JsonResult FindAllTxHash()
        {
            try
            {
                using (var db = new LiteDatabase(string.Concat(AppContext.BaseDirectory, @"App_Data/", "txHash.db")))
                {
                    var CLHash = db.GetCollection<TxHash>("hash");
                    var result = CLHash.FindAll().Distinct().ToList();

                    return new JsonResult(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                //todo: log and return client handlable exception
                throw ex;
            }

            return new JsonResult("[]");
        }
    }
}