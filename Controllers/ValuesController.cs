using AutomationAPI.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Http;

using AutomationAPI.Models.TestRun;

namespace AutomationAPI.Controllers
{
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Get all applications that are currently active.
        /// </summary>
        /// <returns>Returns a distinct list of applications that are currently in an 'active' state.</returns>
        [HttpGet, Route("api/GetApps/")]
        public IHttpActionResult GetApps()
        {
            ApplicationNameTestListModel appTestListModel = new ApplicationNameTestListModel();
            var applications = (IEnumerable<ApplicationNameTestList>)appTestListModel.ApplicationNameTestLists;
            applications = applications.DistinctBy(x => x.ApplicationName).ToList();

            return Ok(applications);
        }

        /// <summary>
        /// Get x amount of test runs, ordered in descending order.
        /// </summary>
        /// <param name="amount">The amount of test runs to get.</param>
        /// <returns>Returns a list of test runs, in a descended order, amount based on amount of test runs based on.</returns>
        [HttpGet, Route("api/GetTestRuns/{amount}")]
        public IHttpActionResult GetTestRuns(int amount)
        {
            vTestRunsFlatModel testRunsModel = new vTestRunsFlatModel();
            var testRuns = testRunsModel.vTestRunsFlats.Take(amount).OrderByDescending(x => x.TestRunKey);

            return Ok(testRuns);
        }


        [HttpGet, Route("api/GetTestResults/{testRunKey}")]
        public IHttpActionResult GetTestResults(int testRunKey)
        {
            TestResultsFlatModel testResultsModel = new TestResultsFlatModel();
            var testResults = testResultsModel.TestResultsFlats.Where(x => x.TestRunKey == testRunKey);

            return Ok(testResults);
        }

        /// <summary>
        /// Get all environments based on a passed in application name.
        /// </summary>
        /// <param name="appName">The application name to get the list of environments available.</param>
        /// <returns>Returns a distinct list of environments that are available for the supplied application.</returns>
        [HttpGet, Route("api/GetEnvironments/{appName}")]
        public IHttpActionResult GetEnvironments(string appName)
        {
            vApplicationEnvironmentsModel vApplicationEnvironmentsModel = new vApplicationEnvironmentsModel();
            var environments = (IEnumerable<vApplicationEnvironment>)vApplicationEnvironmentsModel.vApplicationEnvironments;
            environments = environments.Where(x => x.ApplicationName == appName && x.Active).ToList();

            return Ok(environments);
        }

        /// <summary>
        /// Get all test names based on a passed in application name.
        /// </summary>
        /// <param name="appName">The application name to get the list of tests available.</param>
        /// <returns>Returns a distinct list of tests that are currently active and available for the supplied application.</returns>
        [HttpGet, Route("api/GetTests/{appName}")]
        public IHttpActionResult GetTests(string appName)
        {
            ApplicationNameTestListModel appTestListModel = new ApplicationNameTestListModel();
            var tests = (IEnumerable<ApplicationNameTestList>)appTestListModel.ApplicationNameTestLists;
            tests = tests.Where(x => x.ApplicationName == appName).ToList();

            return Ok(tests);
        }

        /// <summary>
        /// Post to the QueuedTests table for online web execution delivery.
        /// </summary>
        /// <param name="data">Json formatted data that includes the following required fields as well as some preferences: TestName, ApplicationName, Environment, Username.</param>
        /// <returns>Returns either a status code of 200 (succesfully posted) or a status code of 400 (bad request).</returns>
        [Route("api/AddToQueue"), HttpPost]
        public HttpStatusCode AddToQueue([FromBody] string data)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(new StringReader(data));
                QueuedTestExtra myQueueWithExtra = serializer.Deserialize<QueuedTestExtra>(reader);

                ApplicationNameTestListModel applicationNameTestListModel = new ApplicationNameTestListModel();
                var allTestsForApp = applicationNameTestListModel.ApplicationNameTestLists.Where(
                    x => x.ApplicationName == myQueueWithExtra.ApplicationName).ToList();

                QueuedTestsModel myModel = new QueuedTestsModel();
                QueuedTest myQyQueuedTest;

                if (myQueueWithExtra.RunAllApi)
                {
                    var allApiTests = allTestsForApp.Where(x => x.TestName.Contains("API")).ToList();
                    foreach (var item in allApiTests)
                    {
                        myQyQueuedTest = new QueuedTest
                        {
                            Id = myQueueWithExtra.Id,
                            QueuedDateTime = myQueueWithExtra.QueuedDateTime,
                            ApplicationName = myQueueWithExtra.ApplicationName,
                            ConsoleLog = myQueueWithExtra.ConsoleLog,
                            Environment = myQueueWithExtra.Environment,
                            StackTrace = myQueueWithExtra.StackTrace,
                            TestName = item.TestName,
                            UserName = myQueueWithExtra.UserName,
                            Utilization = myQueueWithExtra.Utilization
                        };
                        myModel.QueuedTests.Add(myQyQueuedTest);
                    }


                }
                if (myQueueWithExtra.RunAllGui)
                {

                    var allGuiTests = allTestsForApp.Where(x => !x.TestName.Contains("API")).ToList();
                    foreach (var item in allGuiTests)
                    {
                        myQyQueuedTest = new QueuedTest
                        {
                            Id = myQueueWithExtra.Id,
                            QueuedDateTime = myQueueWithExtra.QueuedDateTime,
                            ApplicationName = myQueueWithExtra.ApplicationName,
                            ConsoleLog = myQueueWithExtra.ConsoleLog,
                            Environment = myQueueWithExtra.Environment,
                            StackTrace = myQueueWithExtra.StackTrace,
                            TestName = item.TestName,
                            UserName = myQueueWithExtra.UserName,
                            Utilization = myQueueWithExtra.Utilization
                        };
                        myModel.QueuedTests.Add(myQyQueuedTest);
                    }
                }
                if (myQueueWithExtra.RunAllApi == false && myQueueWithExtra.RunAllGui == false)
                {
                    myQyQueuedTest = new QueuedTest
                    {
                        Id = myQueueWithExtra.Id,
                        QueuedDateTime = myQueueWithExtra.QueuedDateTime,
                        ApplicationName = myQueueWithExtra.ApplicationName,
                        ConsoleLog = myQueueWithExtra.ConsoleLog,
                        Environment = myQueueWithExtra.Environment,
                        StackTrace = myQueueWithExtra.StackTrace,
                        TestName = myQueueWithExtra.TestName,
                        UserName = myQueueWithExtra.UserName,
                        Utilization = myQueueWithExtra.Utilization
                    };
                    myModel.QueuedTests.Add(myQyQueuedTest);
                }

                myModel.SaveChanges();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }


        /// <summary>
        /// Get all tests currently in queue.
        /// </summary>
        /// <param name="appName">The application name to get the list of tests available.</param>
        /// <returns>Returns a list of tests that are currently in the queue awaiting to be ran.</returns>
        [HttpGet, Route("api/GetQueuedTests")]
        public IHttpActionResult GetQueuedTests()
        {
            QueuedTestsModel queuedTestsModel = new QueuedTestsModel();
            var currentQueuedTests = queuedTestsModel.QueuedTests.ToList();

            return Ok(currentQueuedTests);
        }


        [Route("api/GetPreferences/{appName}/{testerName}/{queue}"), HttpGet]
        public IHttpActionResult GetPreferences(string appName, string testerName, bool queue = false)
        {
            var userPreferences = new vUserPreferenceModel();
            var preferences = new vUserPreference();

            /*
             * 
             * BrowserName
             * Environment
             * EnvironmentUrl
             * Username
             * Stack Trace
             * Console Log
             * Utilization
             * Write to dB
             * 
             */

            if (queue)
            {
                var queuedTest = new QueuedTestsModel();
                var newestQueueItem = queuedTest.QueuedTests.FirstOrDefault();

                var applicationEnvironmentsModel = new vApplicationEnvironmentsModel();
                var applicationEnvironment = applicationEnvironmentsModel.vApplicationEnvironments.FirstOrDefault(
                    x => x.ApplicationName == appName && x.Environment == newestQueueItem.Environment);


                preferences = new vUserPreference
                {
                    TesterName = newestQueueItem.UserName,
                    ApplicationName = newestQueueItem.ApplicationName,
                    Environment = newestQueueItem.Environment,
                    WriteToDatabase = true,
                    StackTrace = newestQueueItem.StackTrace,
                    Utilization = newestQueueItem.Utilization,
                    BrowserConsoleLog = newestQueueItem.ConsoleLog,
                    EnvironmentUrl = applicationEnvironment.Url,
                    RemoteDriver = true
                };
            }
            else
            {
                preferences =
                    userPreferences.vUserPreferences.FirstOrDefault(x => x.TesterName == testerName && x.ApplicationName == appName.ToUpper());
            }

            return Ok(preferences);
        }


        [HttpGet, Route("api/GetExecutedTests/{appName}/{environment}")]
        public IHttpActionResult GetExecutedTests(string appName, string environment)
        {
            var testExecutionListModel = new TestExecutionListModel();
            ExecutedChartData data = new ExecutedChartData
            {
                TestRunResults = environment != "ALL" ?
                testExecutionListModel.TestExecutionLists.Where(x => x.ApplicationName == appName && x.Environment == environment).OrderByDescending(x => x.Status).ToList() :
                    testExecutionListModel.TestExecutionLists.Where(x => x.ApplicationName == appName).OrderByDescending(x => x.Status).ToList()
            };

            //API vs GUI
            data.Executed = data.TestRunResults.Count(x => x.Status.ToUpper().Contains("EXECUTED"));
            data.NotExecuted = data.TestRunResults.Count(x => x.Status.ToUpper() != "EXECUTED");

            return Ok(data);
        }


      

        /// <summary>
        /// Get all test names based on a passed in application name.
        /// </summary>
        /// <param name="appName">The application name to get the list of tests available.</param>
        /// <returns>Returns a distinct list of tests that are currently active and available for the supplied application.</returns>
        [HttpGet, Route("api/VTestRunResults/{appName}/{environment}/{startDateFilter}/{endDateFilter}")]
        public IHttpActionResult VTestRunResults(string appName, string environment, DateTime startDateFilter, DateTime endDateFilter)
        {

            TestRunResultsModel testRunsModel = new TestRunResultsModel();

            endDateFilter = endDateFilter.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            TestRunChartData data = new TestRunChartData
            {
                TestRunResults = environment != "ALL" ?
                testRunsModel.TestRunResult
                    .Where(x => x.ApplicationName == appName && x.Environment == environment && x.StartDateTime >= startDateFilter && x.EndDateTime <= endDateFilter)
                    .OrderByDescending(x => x.ApplicationName)
                    .ToList()
                    :
                    testRunsModel.TestRunResult
                    .Where(x => x.ApplicationName == appName && x.StartDateTime >= startDateFilter && x.EndDateTime <= endDateFilter)
                    .OrderByDescending(x => x.ApplicationName)
                    .ToList()
            };


            data.TestRunResults = data.TestRunResults.Except(data.TestRunResults.Where(x => x.TestRunStatus == null)).ToList();

            // Pass vs Fail
            data.AmountPass = data.TestRunResults.Count(x => x.TestRunStatus.ToUpper() == "PASS");
            data.AmountFail = data.TestRunResults.Count(x => x.TestRunStatus.ToUpper() == "FAIL");

            // API vs GUI
            data.AmountAPI = data.TestRunResults.Count(x => x.TestName.ToUpper().Contains("API"));
            data.AmountGUI = data.TestRunResults.Count(x => x.TestName.ToUpper() != "API");

            ///////////////////////////////////////////End API vs GUI/////////////////////////////
            //                         ______.........--=T=--.........______
            //                                          :|:
            //                      :-.              /""""""\
            //                      ': '-._____..--""(""""""()`---.__
            //                        /:   _.._vr_''   ":""""'[] |""`\\
            //                        ': :'     `-.    _:._      '"""" :
            //                          ::          '--=:____:.___....-"
            //                                           0"       O" 
            ///////////////////////////////////////////Envriroment + Quantity run/////////////////

            var totalEnviroments = data.TestRunResults.Select(x => x.Environment).ToList();
            var distinctEnviroments = totalEnviroments.Distinct().ToList();

            data.EnviromentNameAndCount = new Dictionary<string, double>();
            foreach (var enviroment in distinctEnviroments)
            {
                var value = (double)data.TestRunResults.Count(x => x.Environment.ToUpper() == enviroment.ToUpper());
                var total = (double)totalEnviroments.Count;
                var precent = Math.Round(value, 0) / Math.Round(total, 0);
                var roundedValue = Math.Round(precent * 100, 1);
                data.EnviromentNameAndCount.Add(enviroment.ToUpper(), roundedValue);
            }

            // User + count of times ran
            var distinctUsers = testRunsModel.TestRunResult.Select(x => x.TesterName).Distinct().ToList();
            data.UserNameAndCount = new Dictionary<string, int>();
            foreach (var user in distinctUsers)
            {
                data.UserNameAndCount.Add(user.ToUpper(), data.TestRunResults.Count(x => x.TesterName.ToUpper() == user.ToUpper()));
            }
            var orderedTopUserExec = from pair in data.UserNameAndCount
                                     orderby pair.Value descending
                                     select pair;
            data.UserNameAndCount = orderedTopUserExec.ToDictionary(x => x.Key, x => x.Value);
            //DtPrecent
            var distinctdates = data.TestRunResults.Select(x => x.StartDateTime.ToString("d")).Distinct().ToList();
            var orderedListDistinctdates = distinctdates.OrderBy(x => DateTime.Parse(x)).ToList();
            data.DtPrecent = new Dictionary<string, double>();
            foreach (var date in orderedListDistinctdates)
            {
                var pass = data.TestRunResults.Count(x => x.StartDateTime.ToString("d") == date && x.TestRunStatus == "Pass");
                var total = data.TestRunResults.Count(x => x.StartDateTime.ToString("d") == date);
                var precent = (Math.Round((double)pass, 0) / Math.Round((double)total, 0));
                var roundedValue = Math.Round(precent * 100, 0);              
                data.DtPrecent.Add(date, roundedValue);
            }
            //var orderedTopDates = from pair in data.DtPrecent
            //                      orderby pair.Key ascending
            //                      select pair;
          
           // data.DtPrecent = orderedTopDates.ToDictionary(x => x.Key, x => x.Value);

            // Favorite Test && Testname and fail count
            var distinctTestNamesList = data.TestRunResults.Select(x => x.TestName).Distinct().ToList();
            data.TestNameAndCount = new Dictionary<string, int>();
            data.TestNameAndFailCount = new Dictionary<string, int>();
            foreach (var testName in distinctTestNamesList)
            {
                data.TestNameAndCount.Add(testName, data.TestRunResults.Count(x => x.TestName.ToUpper() == testName.ToUpper()));
                data.TestNameAndFailCount.Add(testName, data.TestRunResults.Count(x => x.TestName.ToUpper() == testName.ToUpper() && x.TestRunStatus.ToUpper() == "FAIL"));
            }
            var orderedTopFavs = from pair in data.TestNameAndCount
                                 orderby pair.Value descending
                                 select pair;
            data.TestNameAndCount = orderedTopFavs.ToDictionary(x => x.Key, x => x.Value);

            var orderedTopFails = from pair in data.TestNameAndFailCount
                                  orderby pair.Value descending
                                  select pair;
            data.TestNameAndFailCount = orderedTopFails.ToDictionary(x => x.Key, x => x.Value);


            var orderedFavoriteTests = from pair in data.TestNameAndCount
                                       orderby pair.Value descending
                                       select pair;
            data.TestNameAndCount = orderedFavoriteTests.ToDictionary(x => x.Key, x => x.Value);


            return Ok(data);
        }


        [HttpGet, Route("api/GenerateEmail/{appName}/{daysBack}/{environment}")]
        public HttpStatusCode GenerateEmail(string appName, int daysBack, string environment)
        {
            try
            {
                DateTime today = DateTime.Today;


                ApplicationNameTestListModel applicationNameTestList = new ApplicationNameTestListModel();
                var testList = applicationNameTestList.ApplicationNameTestLists.Where(x => x.ApplicationName == appName);

                vUniqueExecutedTestsModel uniqueExecutedTestsModel = new vUniqueExecutedTestsModel();
                var executedTests = uniqueExecutedTestsModel.vUniqueExecutedTests.Where(x => x.ApplicationName == appName && x.Environment.ToUpper() == environment);

                if (daysBack > 0)
                {
                    today = today.AddDays(0 - daysBack);
                    executedTests = executedTests.Where(x => x.StartDateTime >= today);
                }

                #region Set the Email From, To Section and SMTP

                MailMessage mail = new MailMessage();
                MailAddress fromMail = new MailAddress("AutomationOutput@nelnet.net");
                mail.From = fromMail;

                // TODO: Update to get selected user groups from email distribution.
                mail.To.Add("z-ITQAAutomation@nelnet.net");
                mail.To.Add("z-5280mmav3@nelnet.net");

                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "Exchcasprd01v.us.nelnet.biz";

                #endregion Set the Email From, To Section and SMTP

                #region Format the Email Body

                mail.IsBodyHtml = true;
                mail.Body = "<div style='font-family:Calibri;'>";

                #region Style

                mail.Body += "<style>" +
                             "table, th, td {" +
                             "border: 1px solid black;" +
                             "border-collapse: collapse;" +
                             "}" +
                             "th, td {" +
                             "padding: 2px;" +
                             "text-align: left;" +
                             "}" +
                             "table#t01 tr:nth-child(even) {" +
                             "background-color: #eee;" +
                             "}" +
                             "table#t01 tr:nth-child(odd) {" +
                             "background-color: #fff;" +
                             "}" +
                             "table#t01 th {" +
                             "background-color: #4d6fa5;" +
                             "color: white;" +
                             "}" +
                             "</style>";

                #endregion Style

                mail.Body += $"<h2>{appName} - {environment}</h2>";
                mail.Body += "<table id=\"t01\">" +
                                   "<tr>" +
                                     "<th>Type</th>" +
                                     "<th># of passes </th> " +
                                     "<th># of failures</th>" +
                                     "<th># of not run</th>" +
                                     "<th># of total tests</th>" +
                                 "</tr>" +
                                 "<tr>" +
                                     "<td>Number of UI tests</td>" +
                                     $"<td>{executedTests.Except(executedTests.Where(x => x.TestName.Contains("API"))).Count(x => x.TestRunStatus.ToUpper() == "PASS")}</td>" +
                                     $"<td>{executedTests.Except(executedTests.Where(x => x.TestName.Contains("API"))).Count(x => x.TestRunStatus.ToUpper() == "FAIL")}</td>" +
                                     $"<td>{testList.Except(testList.Where(x => x.TestName.Contains("API"))).Count() - (executedTests.Except(executedTests.Where(x => x.TestName.Contains("API"))).Count(x => x.TestRunStatus.ToUpper() == "PASS") + executedTests.Except(executedTests.Where(x => x.TestName.Contains("API"))).Count(x => x.TestRunStatus.ToUpper() == "FAIL"))}</td>" +
                                     $"<td>{testList.Except(testList.Where(x => x.TestName.Contains("API"))).Count()}</td> " +
                                 "</tr> " +
                                 "<tr> " +
                                     "<td>Number of API tests</td> " +
                                     $"<td>{executedTests.Where(x => x.TestName.Contains("API")).Count(x => x.TestRunStatus.ToUpper() == "PASS")}</td> " +
                                     $"<td>{executedTests.Where(x => x.TestName.Contains("API")).Count(x => x.TestRunStatus.ToUpper() == "FAIL")}</td> " +
                                     $"<td>{testList.Count(x => x.TestName.Contains("API")) - (executedTests.Where(x => x.TestName.Contains("API")).Count(x => x.TestRunStatus.ToUpper() == "PASS") + executedTests.Where(x => x.TestName.Contains("API")).Count(x => x.TestRunStatus.ToUpper() == "FAIL"))}</td> " +
                                     $"<td>{testList.Count(x => x.TestName.Contains("API"))}</td> " +
                                 "</tr> " +
                             "</table>";

                mail.Body += "------</br>";
                mail.Body += "<p>To see the full results please <a href=\"http://autodash.nelnet.net\">click here.</a>";
                mail.Body += "</div>";

                #endregion Format the Email Body

                mail.Subject = $"Automated Report for {appName} against {environment}";
                client.Send(mail);

                return HttpStatusCode.OK;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPost]
        [Route("api/CreateTestResult")]
        public HttpStatusCode CreateTestResult([FromBody] string data)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(new StringReader(data));
                TestResultsTable result = serializer.Deserialize<TestResultsTable>(reader);

                TestResultsTableModel myModel = new TestResultsTableModel();

                myModel.TestResultsTables.AddOrUpdate(result);
                myModel.SaveChanges();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPost]
        [Route("api/CreateTestRun")]
        public IHttpActionResult CreateTestRun([FromBody] string data)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(new StringReader(data));
                TestRunsTable run = serializer.Deserialize<TestRunsTable>(reader);

                TestRunsTableModel myModel = new TestRunsTableModel();

                myModel.TestRunsTables.AddOrUpdate(run);
                myModel.SaveChanges();

                return Ok(myModel.TestRunsTables.ToList().LastOrDefault().TestRunKey);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/CreateTest")]
        public IHttpActionResult CreateTest([FromBody] string data)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(new StringReader(data));
                TestList test = serializer.Deserialize<TestList>(reader);

                TestListModel testListModel = new TestListModel();

                testListModel.TestLists.AddOrUpdate(test);
                testListModel.SaveChanges();



                return Ok(testListModel.TestLists.ToList().LastOrDefault().TestListKey);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/GetTestKey/{appName}/{testName}")]
        public IHttpActionResult GetTestKey(string appName, string testName)
        {
            try
            {
                ApplicationNameTestListModel appTestListModel = new ApplicationNameTestListModel();
                var test = appTestListModel.ApplicationNameTestLists;
                return Ok(test.FirstOrDefault(x => x.ApplicationName == appName && x.TestName == testName).TestListKey);
            }
            catch
            {
                return BadRequest("Test name not found");
            }
        }

        [HttpGet]
        [Route("api/GetAppKey/{appName}")]
        public IHttpActionResult GetAppKey(string appName)
        {
            try
            {
                ApplicationsModel appModel = new ApplicationsModel();
                return Ok(appModel.Applications.FirstOrDefault(x => x.ApplicationName == appName).ApplicationKey);
            }
            catch
            {
                return BadRequest("App name not found");
            }
        }


    }
}
