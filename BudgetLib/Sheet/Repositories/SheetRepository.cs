using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using Sheet.Common.Interfaces;
using Microsoft.Extensions.Options;
using Sheet.Common.Models;
using Sheet.Common.Enums;

namespace Sheet.Common
{
    public class SheetRepository : ISheetRepository
    {
        private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private SheetsService _googleSheets;
        public SheetRepository(IOptions<GoogleServiceAccount> serviceAccount)
        {
            var _serviceAccount = serviceAccount.Value;
            ServiceAccountCredential credential;

            if (string.IsNullOrEmpty(_serviceAccount.ServiceEmail))
            {
                Console.WriteLine($"GOOGLE EMAIL NOT PROVIDED: {_serviceAccount.ServiceEmail}");
                throw new ArgumentNullException(nameof(_serviceAccount.ServiceEmail));
            }


            if (string.IsNullOrEmpty(_serviceAccount.PrivateKey))
            {
                Console.WriteLine($"GOOGLE PRIVATE KEY NOT PROVIDED: {_serviceAccount.PrivateKey}");
                throw new ArgumentNullException(nameof(_serviceAccount.PrivateKey));
            }

            var initializer = new ServiceAccountCredential.Initializer(_serviceAccount.ServiceEmail)
            {
                Scopes = Scopes
            };

            credential = new ServiceAccountCredential(initializer.FromPrivateKey(_serviceAccount.PrivateKey));

            if (!credential.RequestAccessTokenAsync(System.Threading.CancellationToken.None).Result)
                throw new InvalidOperationException("Access token failed.");

            // create Google Sheets API service
            _googleSheets = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Sheets API .NET Quickstart"
            });


        }

        // TODO: add batchGet implementation for more optimized queries
        public IList<IList<Object>> LoadRange(string spreadsheetId, string range)
        {
            if (_googleSheets == null)
                throw new ApplicationException("Could not establish connection to GoogleAPI.");

            var request = _googleSheets.Spreadsheets.Values.Get(spreadsheetId, range);
            return request.Execute().Values;
        }

        public bool UpdateRange(string spreadsheetId, RangeUpdateModel model)
        {
            var valueRange = new ValueRange();
            valueRange.MajorDimension = model.Dimension.GetString();
            valueRange.Values = model.Values;

            var updateRequest = _googleSheets.Spreadsheets.Values.Update(
                valueRange,
                spreadsheetId,
                model.Range
            );
            updateRequest.ValueInputOption = SpreadsheetsResource
                .ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            var response = updateRequest.Execute();
            Console.WriteLine($"Cells Updated: {response.UpdatedCells}");
            return (response.UpdatedCells ?? 0) > 0;
        }

        public bool UpdateRange(string spreadsheetId, IList<RangeUpdateModel> models)
        {
            var data = new List<ValueRange>();
            foreach (var model in models)
            {
                var valueRange = new ValueRange();
                valueRange.Values = model.Values;
                valueRange.MajorDimension = model.Dimension.GetString();
                valueRange.Range = model.Range;
                data.Add(valueRange);
            }

            var requestBody = new BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "USER_ENTERED"; // Google's magic string
            requestBody.Data = data;

            var request = _googleSheets.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);
            var response = request.Execute();
            return (response.TotalUpdatedCells ?? 0) > 0;
        }

        public bool ClearRange(string spreadsheetId, string range)
        {
            var requestBody = new Google.Apis.Sheets.v4.Data.ClearValuesRequest();
            var clearRequest = _googleSheets.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range);
            var response = clearRequest.Execute();
            return true;
        }

        // TODO: figire out how this could be useful
        // public bool AppendToRange(string spreadsheetId, RangeUpdateModel model)
        // {
        //     var valueInputOption = SpreadsheetsResource.ValuesResource
        //         .AppendRequest.ValueInputOptionEnum.USERENTERED;
        //     var insertDataOption = SpreadsheetsResource.ValuesResource
        //         .AppendRequest.InsertDataOptionEnum.INSERTROWS;

        //     var valueRange = new ValueRange();
        //     valueRange.Values = model.Values;
        //     valueRange.MajorDimension = model.Dimension.GetString();
        //     valueRange.Range = model.Range;

        //     var request = _googleSheets.Spreadsheets.Values.Append(valueRange, spreadsheetId, model.Range);
        //     request.ValueInputOption = valueInputOption;
        //     request.InsertDataOption = insertDataOption;
        //     var response = request.Execute();
        //     return (response.Updates.UpdatedCells ?? 0) > 0;
        // }
    }
}



