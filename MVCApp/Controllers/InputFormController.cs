using System.Diagnostics;
using BinanceBotNuGetPackage.BotExecutor;
using BinanceBotNuGetPackage.Helpers;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;



namespace MVCApp.Controllers;

public class InputFormController : Controller
{
    private readonly ILogger<InputFormController> _logger;
    private UiPathParams _uiPathParams { get; set; } 
    private string _connectionString { get; set; }

    public InputFormController(ILogger<InputFormController> logger)
    {
        _logger = logger;
    }
    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult PrepareUiPathExecution(UiPathParams? model)
    {
        try
        {
            if (model == default)
            {
                throw new AggregateException("Please return to form and fill in all the gaps");
            }
            _uiPathParams = model;
            _connectionString = Path.Combine(Directory.GetCurrentDirectory(), "BinanceBotDb.db");
            TempData["startDate"] = DateTime.Parse(model.StartDate);
            TempData["endDate"] = DateTime.Parse(model.EndDate);
            TempData["period"] = model.PeriodSelector;
            TempData["conStr"] = _connectionString;
            TempData["cryptoName"] = model.Name;
            if (!model.FromDb)
            {
                string flowFolderName = Directory.GetCurrentDirectory().Split("\\").Last();
                // var flowFileName = Directory
                //     .GetFiles(@"C:\Users\Viktor_Romashko\Desktop\diplomaproj\BinanceBot\PublishedNuGetsOfBot")
                //     .OrderByDescending(x=>x)
                //     .First()
                //     .Split("\\")
                //     .Last();
                var flowFileName = "BinanceBot.1.0.37.nupkg";
                var uiPathExecutor = new AutomationExecutor(flowFolderName);
                uiPathExecutor.ExecuteProcessWithNotParsedOutput(
                    flowFileName,
                    model.Name,
                    model.StartDate,
                    model.EndDate,
                    model.PeriodSelector,
                    FinderHelper
                        .ReplaceSpacesAndBackslashes($"{Directory.GetCurrentDirectory()}\\Untitled.txt")
                );
        
                var data=FinderHelper.ReadFromFileAndParse($"{Directory.GetCurrentDirectory()}\\Untitled.txt");
                var tradingPair = data.Last();
                var last_ind = data.IndexOf(data.Last());
        
                data.RemoveAt(last_ind);
                FinderHelper.InsertFullCryptoIntoDb(
                    data,
                    model.Name,
                    tradingPair,
                    model.PeriodSelector,
                    DateTime.Parse(model.StartDate),
                    DateTime.Parse(model.EndDate),
                    _connectionString
                );
            }

            FinderHelper.RemoveDuplicates(_connectionString);
            return RedirectToAction("UiPath");
        }
        catch (Exception e)
        {
            return View("EmptyUiPath",new ErrorViewModel { ErrorMessage = e.Message });
        }
        
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult UiPath()
    {
        try
        {
            var conStr = TempData["conStr"] as string;
            string startD = TempData["startDate"].ToString();
            string endD = TempData["endDate"].ToString();
            var period = TempData["period"] as string;
            var name = TempData["cryptoName"].ToString();
        
            var dataForCart = FinderHelper.ReturnDataByPeriod(
                DateTime.Parse(startD),
                DateTime.Parse(endD),
                period,
                name,
                conStr
            );
            if (dataForCart.Count == 0)
            {
                return View("EmptyUiPath",new ErrorViewModel
                {
                    ErrorMessage = "There is no such data in database." +
                                   " Please consider to repeat your request, but without 'From Database' checked."
                });
            }
            var chartModel = new ChartModel
            {
                Deals = dataForCart,
                StartDate = DateTime.Parse(startD),
                EndDate = DateTime.Parse(endD),
            };
            return View(chartModel);
        }
        catch (Exception e)
        {
            return View("EmptyUiPath",new ErrorViewModel { ErrorMessage = e.Message });
        }
    }
    public IActionResult EmptyUiPath(ErrorViewModel error)
    {
        return View(error);
    }
}