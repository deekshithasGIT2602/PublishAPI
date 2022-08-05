using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PublishAPI.Models;
using PublishAPI.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Serilog;
using StackExchange.Profiling;
using PublishAPI.Middleware;
using PublishAPI.Helpers;
using Confluent.Kafka;
using KafkaNet.Model;
using KafkaNet;
using KafkaNet.Protocol;

namespace PublishAPI.Repository
{
    public class DataRepository : IDataRepository
    {
        private readonly IMongoCollection<MT4040_VoterFramework> _data;
        private readonly IOptions<DatabaseSettings> _settings;

        public DataRepository(IOptions<DatabaseSettings> settings, IOptions<UserSettings> config)
        {
            _settings = settings;
            var connection = new MongoClient(_settings.Value.ConnectionString);
            var database = connection.GetDatabase(_settings.Value.DatabaseName);
            _data = database.GetCollection<MT4040_VoterFramework>(_settings.Value.CollectionName);
        }

        public ActionResult GetVoterid(VoterIDRequest voteridRequest)
        {
            var payload = JsonConvert.SerializeObject(voteridRequest);
            var transId = Guid.NewGuid().ToString();
            if (AuditMiddleware.Logger != null)
            {
                AuditLogger.RequestInfo(
                transId, Constants.Post, Constants.Path, string.Empty, voteridRequest.ToString());
            }
            if (AuditMiddleware.Logger != null)
            {
                AuditLogger.ResponseInfo(transId, Constants.Post, Constants.Path, string.Empty, _settings.Value.DatabaseName, _settings.Value.CollectionName, payload);
            }
            Log.Information("Request: {0}", JsonConvert.SerializeObject(voteridRequest));
            string voterid = voteridRequest.voterid;
            return Get(voterid);
        }
        public ActionResult Get(string voterid)
        {
            Log.Information("Database Connected Successfully");
            List<MT4040_VoterFramework> records = new List<MT4040_VoterFramework>();
            List<ResponseDTO> list = new List<ResponseDTO>();
            string V_id = "";
            using (MiniProfiler.Current.Step("Time taken to retrieve data from database:"))
            {
                if (voterid.Length > 0)
                {
                    records = _data.Find(book => book.voterid == voterid).ToList();
                    V_id = voterid;
                }
                foreach (var record in records)
                {
                    ResponseDTO res = new ResponseDTO();
                    res.Rollno_flag = record.rollno_flag;
                    res.Pb_code = record.pb_code;
                    res.Guardianname = record.guardianname;
                    res.Gender = record.gender;
                    res.Rollno = record.rollno;
                    res.Pagenum = record.pagenum;
                    res.Version = record.version;
                    res.Sectiono = record.sectiono;
                    res.Guardiantype = record.guardiantype;
                    res.Ecino = record.ecino;
                    res.Stac = record.stac;
                    res.Stacpb = record.stacpb;
                    res.Name = record.name;
                    res.Houseno = record.houseno;
                    res.Rollno_text = record.rollno_text;
                    res.Age = record.age;
                    res.Ac_code = record.ac_code;
                    V_id = record.voterid;
                    list.Add(res);
                    Console.WriteLine(list.Count);
                }
            }
            if (list.Count >= 2)
            {
                Log.Information("More than one match found");
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.ErrorMessage = "More than one match found";
                errorResponse.StatusCode = 400;
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(errorResponse),
                    ContentType = Constants.JSON_CONTENT,
                    StatusCode = 400
                };
            }
            else if (list.Count == 0)
            {
                Log.Information("No matches found");
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.ErrorMessage = "No matches found";
                errorResponse.StatusCode = 404;
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(errorResponse),
                    ContentType = Constants.JSON_CONTENT,
                    StatusCode = 404
                };
            }
            else
            {
                Log.Information("Data was successfully published");
                Log.Information("The Rollno_flag is {0}", list[0].Rollno_flag);
                Log.Information("The Pb_code is {0}", list[0].Pb_code);
                Log.Information("The Guardianname is {0}", list[0].Guardianname);
                Log.Information("The Gender is {0}", list[0].Gender);
                Log.Information("The Rollno is {0}", list[0].Rollno);
                Log.Information("The Pagenum is {0}", list[0].Pagenum);
                Log.Information("The Version is {0}", list[0].Version);
                Log.Information("The Sectiono is {0}", list[0].Sectiono);
                Log.Information("The Guardiantype is {0}", list[0].Guardiantype);
                Log.Information("The Ecino is {0}", list[0].Ecino);
                Log.Information("The Stac is {0}", list[0].Stac);
                Log.Information("The Stacpb is {0}", list[0].Stacpb);
                Log.Information("The Name is {0}", list[0].Name);
                Log.Information("The Houseno is {0}", list[0].Houseno);
                Log.Information("The Rollno_text is {0}", list[0].Rollno_text);
                Log.Information("The Age is {0}", list[0].Age);
                Log.Information("The Ac_code is {0}", list[0].Ac_code);

               // KAfka(list[0]);

                Console.WriteLine("Message is published to Kafka Topic");
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(list),
                    ContentType = Constants.JSON_CONTENT,
                    StatusCode = 200
                };
            }
        }

        public async Task KAfka(ResponseDTO responseDTO)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    string message = JsonConvert.SerializeObject(responseDTO);
                    var dr = await p.ProduceAsync("MT4073_publish", new Message<Null, string> { Value = message });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
        

        ///Return health status of API

        public async Task<bool> IsAliveAsync()
        {
            try
            {
                using (MiniProfiler.Current.Step(Constants.HealthCommand))
                {
                    await Task.Delay(1);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }
    }
}

