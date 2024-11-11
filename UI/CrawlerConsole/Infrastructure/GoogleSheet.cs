using CrawlerConsole.Domain;
using CrawlerConsole.Helper;
using CrawlerConsole.Interfaces;
using CrawlerConsole.Mapper;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace CrawlerConsole.Infrastructure
{
    public class GoogleSheet : IGoogleSheet
    {

        //private SpreadsheetsResource.ValuesResource _googleSheetValues;
        private SheetsService _googleSheetsService;
        private readonly SettingsModel? _settings;

        public GoogleSheet(GoogleSheetsHelper googleSheetsHelper, IConfiguration configuration)
        {
            _googleSheetsService = googleSheetsHelper.Service;
            //_googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            _settings = configuration.GetSection("Settings").Get<SettingsModel>();
        }

        public async Task<IList<IList<object>>?> GetGoogleSheetDataAsync()
        {
            IList<IList<object>>? lists = null;
            if (_settings == null)
            {
                return lists;
            }

            try
            {
                var range = $"{_settings.GoogleSheet.Name}{_settings.GoogleSheet.ColumnRange}";
                //var request = _googleSheetValues.Get(_settings.GoogleSheet.SpreadId, range);
                var request = _googleSheetsService.Spreadsheets.Values.Get(_settings.GoogleSheet.SpreadId, range);
                request.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
                request.DateTimeRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.DateTimeRenderOptionEnum.FORMATTEDSTRING;
                request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.DIMENSIONUNSPECIFIED;
                var response = await request.ExecuteAsync();
                lists = response.Values;
            }
            catch (Exception ex)
            {
                lists = null;
                var msg = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    msg += ", Inner: " + ex.InnerException.Message;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR (GetGoogleSheetDataAsync) => {msg}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                //_logger.Error("GetGoogleSheetData: " + msg);
            }

            return lists;
        }

        public async Task<bool> WriteGoogleSheetDataAsync(string sheetName, IList<IList<object>> values)
        {
            if (_settings == null)
            {
                return false;
            }
            try
            {
                var range = $"{sheetName}{_settings.GoogleSheet.ColumnRange}";
                var valueRange = new ValueRange
                {
                    Values = values
                };

                //var appendRequest = _googleSheetValues.Append(valueRange, _settings.GoogleSheet.SpreadId, range);
                var appendRequest = _googleSheetsService.Spreadsheets.Values.Append(valueRange, _settings.GoogleSheet.SpreadId, range);
                appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
                //appendRequest.Execute();
                var response = await appendRequest.ExecuteAsync();
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    msg += ", Inner: " + ex.InnerException.Message;
                }
                //_logger.Error("GetGoogleSheetData: " + msg);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR (WriteGoogleSheetDataAsync) => {msg}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public async Task<bool> WriteBatchGoogleSheetDataAsync(string sheetName, IList<IList<object>> values)
        {
            if (_settings == null)
            {
                return false;
            }
            try
            {
                var range = $"{sheetName}{_settings.GoogleSheet.ColumnRange}";
                var valueRange = new ValueRange
                {
                    Values = values
                };
                var updateRequest = _googleSheetsService.Spreadsheets.Values.Update(valueRange, _settings.GoogleSheet.SpreadId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                var updateResponse = await updateRequest.ExecuteAsync();

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    msg += ", Inner: " + ex.InnerException.Message;
                }
                //_logger.Error("GetGoogleSheetData: " + msg);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR (WriteBatchGoogleSheetDataAsync) => {msg}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public async Task<bool> CreateNewGoogleSheet(string title)
        {
            try
            {
                // Create a new sheet request
                var addSheetRequest = new AddSheetRequest
                {
                    Properties = new SheetProperties
                    {
                        Title = title
                    }
                };

                var requestBody = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new Request[]
                            {
                            new Request { AddSheet = addSheetRequest }
                            }
                };

                var batchUpdateRequest = _googleSheetsService.Spreadsheets.BatchUpdate(requestBody, _settings.GoogleSheet.SpreadId);
                await batchUpdateRequest.ExecuteAsync();

                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    msg += ", Inner: " + ex.InnerException.Message;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR (CreateNewGoogleSheet) => {msg}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
                return false;
            }
        }
    }
}
