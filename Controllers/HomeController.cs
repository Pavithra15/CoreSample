using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspnetcore3._0.Models;
using Syncfusion.EJ2.Base;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Abp.UI;
using System.Collections;

namespace aspnetcore3._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = BigData.GetAllRecords();
            var model = new CountryViewModel();
            model.Country = "CA";
            return View(model);
        }
        public class CountryViewModel
        {
            public string Country { get; set; }

            public List<SelectListItem> Countries { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico" },
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "USA"  },
        };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UrlDatasource([FromBody] DataManagerRequest dm)
        {
            IEnumerable DataSource = BigData.GetAllRecords();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<BigData>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public class CustValue{
            public string value { get; set; }
            }
        public IActionResult Update([FromBody] CRUDModel<BigData> value)
        {
            //var Order = OrdersDetails.GetAllRecords();
            var data = BigData.GetAllRecords().Where(or => or.OrderID == value.Value.OrderID).FirstOrDefault();
            if (data != null)
            {
                data.OrderID = value.Value.OrderID;
                data.CustomerID = value.Value.CustomerID;
                value.Value.ShipCountry = data.ShipCountry = "Updated";
                data.Verified = value.Value.Verified;
            }

            return Json(value.Value);
        }
        public IActionResult Insert([FromBody] CRUDModel<BigData> value)
        {
            var Order = BigData.GetAllRecords();
            var obj = BigData.GetAllRecords().Where(or => or.OrderID.Equals(int.Parse(value.Value.OrderID.ToString()))).FirstOrDefault();
            BigData.GetAllRecords().Insert(0, value.Value);
            return Json(value.Value);
        }
        public IActionResult Delete([FromBody] CRUDModel<BigData> value)
        {
            var data = BigData.GetAllRecords().Where(or => or.OrderID.Equals(int.Parse(value.Key.ToString()))).FirstOrDefault();
            BigData.GetAllRecords().Remove(data);
            return Json(value);
        }

        public class SyncfusionOperationModel<T> where T : class

        {

            [JsonProperty("action")]

            public string Action { get; set; }

            [JsonProperty("table")]

            public string Table { get; set; }

            [JsonProperty("keyColumn")]

            public string KeyColumn { get; set; }

            [JsonProperty("key")]

            public object Key { get; set; }

            [JsonProperty("value")]

            public T Value { get; set; }

            [JsonProperty("added")]

            public IList<T> Added { get; set; }

            [JsonProperty("changed")]

            public IList<T> Changed { get; set; }

            [JsonProperty("deleted")]

            public IList<T> Deleted { get; set; }

            [JsonProperty("params")]

            public IDictionary<string, object> @params { get; set; }

        }

        public IActionResult UrlDataSource([FromBody]DataManagerRequest dm)
        {
            IEnumerable<SampleData> DataSource = SampleData.GetAllRecords().AsEnumerable();
            DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<SampleData>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public ActionResult Insert([FromBody] CRUDModel<OrdersData> value)
        {
            OrdersData.GetAllRecords().Insert(0, value.Value);
            return Json(value.Value);
        }


        public IActionResult UrlDropDatasource([FromBody]DataManagerRequest dm)
        {
            IEnumerable<OrdersData> DataSource = OrdersData.GetAllRecords();
            return Json(DataSource);
        }

        public class SampleData
        {
            public static List<SampleData>
    sample1 = new List<SampleData>
        ();
            public SampleData()
            {

            }
            public SampleData(string id, string data, string hora, string codigoitem, int descricao)
            {
                this.id = id;
                this.data = data;
                this.hora = hora;
                this.codigoitem = codigoitem;
                this.descricao = descricao;
            }
            public static List<SampleData>
                GetAllRecords()
            {
                if (sample1.Count() == 0)
                {
                    int code = 1;
                    for (int i = 1; i < 3; i++)
                    {
                        sample1.Add(new SampleData("abc", "2023-11-20T00:00:00", "22:00:00", "2182", 30));
                        sample1.Add(new SampleData("cde", "2023-11-20T00:00:00", "22:00:00", "2182", 40));
                        sample1.Add(new SampleData("fgh", "2023-11-20T00:00:00", "22:00:00", "2182", 50));
                        sample1.Add(new SampleData("ijk", "2023-11-20T00:00:00", "22:00:00", "2182", 60));
                        code += 5;
                    }
                }
                return sample1;
            }
           
            public string id { get; set; }
            public string data { get; set; }
            public string hora { get; set; }

            public string codigoitem { get; set; }
            public int descricao { get; set; }

        }

        public class OrdersData
        {
            public static List<OrdersData>
    order1 = new List<OrdersData>
        ();
            public OrdersData()
            {

            }
            public OrdersData(int? UnitID, int? DocumentId, string Category, string FileName, string FilePath, string UploadByUser, DateTime? DateCreated, DateTime? DateModified, string Comments, string Tags,string CommentHistory, string TagHistory, string SubType, string CombinedOrg, string SubFundingYear)
            {
                this.UnitID = UnitID;
                this.DocumentId = DocumentId;
                this.Category = Category;
                this.FileName = FileName;
                this.FilePath = FilePath;
                this.UploadByUser = UploadByUser;
                this.DateCreated = DateCreated;
                this.DateModified = DateModified;
                this.Comments = Comments;
                this.Tags = Tags;
                this.CommentHistory = CommentHistory;
                this.TagHistory = TagHistory;
                this.SubType = SubType;
                this.CombinedOrg = CombinedOrg;
                this.SubFundingYear = SubFundingYear;
        }
            public static List<OrdersData>
                GetAllRecords()
            {
                if (order1.Count() == 0)
                {
                    int code = 1;
                    for (int i = 1; i < 3; i++)
                    {
                        order1.Add(new OrdersData(code + 0, code + 0, "Staffing", "testfile1", "c/newfolder1", "yes", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "test", "test"));
                        order1.Add(new OrdersData(code + 1, code + 1, "Goals", "testfile2", "c/newfolder2", "no", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "test", "test"));
                        order1.Add(new OrdersData(code + 2, code + 2, "Wireless", "testfile3", "c/newfolder3", "yes", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "test", "KI"));
                        order1.Add(new OrdersData(code + 3, code + 3, "Funding", "testfile4", "c/newfolder4", "no", new DateTime(), new DateTime(), "test", "test", "test", "test", "F", "A", "GT"));
                        order1.Add(new OrdersData(code + 4, code + 4, "test5", "testfile5", "c/newfolder5", "yes", new DateTime(), new DateTime(), "test", "test", "test", "test", "A", "A", "test"));
                        order1.Add(new OrdersData(code + 5, code + 5, "Staffing", "testfile1", "c/newfolder1", "yes", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "test", "test"));
                        order1.Add(new OrdersData(code + 6, code + 6, "Goals", "testfile2", "c/newfolder2", "no", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "test", "test"));
                        order1.Add(new OrdersData(code + 7, code + 7, "Wireless", "testfile3", "c/newfolder3", "yes", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "test", "test"));
                        order1.Add(new OrdersData(code + 8, code + 8, "Funding", "testfile4", "c/newfolder4", "no", new DateTime(), new DateTime(), "test", "test", "test", "test", "F", "AJ", "KI"));
                        order1.Add(new OrdersData(code + 9, code + 9, "test5", "testfile5", "c/newfolder5", "yes", new DateTime(), new DateTime(), "test", "test", "test", "test", "test", "AJ", "GT"));
                        code += 5;
                    }
                }
                return order1;
            }
            public int? UnitID { get; set; }
            public int? DocumentId { get; set; }
            public string Category { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string UploadByUser { get; set; }
            public DateTime? DateCreated { get; set; }
            public DateTime? DateModified { get; set; }
            public string Comments { get; set; }

            public string Tags { get; set; }
            public string CommentHistory { get; set; }
            public string TagHistory { get; set; }
            public string SubType { get; set; }
            public string CombinedOrg { get; set; }
            public string SubFundingYear { get; set; }
            //  public string ShipAddress { get; set; }

        }
        public class Customer
        {
            public static List<Customer> order1 = new List<Customer>();
            public Customer()
            {

            }

            public int? CustomerId { get; set; }
            public string Name { get; set; }
        }

        public class BigData
        {
            public static List<BigData> order = new List<BigData>();
            public BigData()
            {

            }
            public BigData(int OrderID, string EntryTime, int Key, int N2, string CustomerID, string EmailID, int QuestionTypeId, double Freight, bool Verified, DateTime? OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime? ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.EntryTime = EntryTime;
                this.Key = Key;
                this.N2 = N2;
                this.CustomerID = CustomerID;
                this.EmailID = EmailID;
                this.QuestionTypeId = QuestionTypeId;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }
            public static List<BigData> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 2; i++)
                    {
                        order.Add(new BigData(code + 1, "1000", 15, 10, "ALFKI", "nancy@domain.com", 4, 1112.3 * i, false, DateTime.Now, "#ff00ff", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                        order.Add(new BigData(code + 2, "1100", 20, 8, "ANATR", "anatr@domain.com", 2, 456433.3 * i, true, new DateTime(2023, 09, 26), "#ffee00", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                        order.Add(new BigData(code + 3, "1515", 22, 15, "ANTON", "anton@domain.com", 1, 6544.3 * i, true, new DateTime(1957, 11, 30), "#110011", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new BigData(code + 4, "0900", 18, 11, "BLONP", "blonp@domain.com", 3, 455.3 * i, false, new DateTime(1930, 10, 22), "#ff5500", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                        order.Add(new BigData(code + 5, "2030", 26, 13, "BOLID", "bolid@domain.com", 4, 63.3 * i, true, new DateTime(2023, 09, 26), "#aa0088", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                        code += 5;
                    }
                }
                return order;
            }
            public int? OrderID { get; set; }
            public string EntryTime { get; set; }
            public int? Key { get; set; }
            public int? N2 { get; set; }
            public string CustomerID { get; set; }
            public string EmailID { get; set; }
            public int? QuestionTypeId { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime? OrderDate { get; set; }
            public string ShipName { get; set; }
            public string ShipCountry { get; set; }
            public DateTime? ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }
    }
}
