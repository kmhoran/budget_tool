using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Sheet.Common.Interfaces;
using Microsoft.Extensions.Options;
using Sheet.Common.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace Sheet.Common
{
    public class SheetRepository : ISheetRepository
    {
        private readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        // private GoogleConfig _config;
        private SheetsService _googleSheets;
        public SheetRepository(IOptions<GoogleConfig> secrets)
        {
            var _config = secrets.Value;
            Console.WriteLine("CONFIG: ");
            Console.WriteLine($"    {_config.EnvironmentName}");
            ServiceAccountCredential credential;

            var initializer = new ServiceAccountCredential.Initializer(_config.ServiceEmail)
            {
                Scopes = Scopes
            };

            credential = new ServiceAccountCredential(initializer.FromPrivateKey(_config.PrivateKey));

            if (!credential.RequestAccessTokenAsync(System.Threading.CancellationToken.None).Result)
                throw new InvalidOperationException("Access token failed.");

            // create Google Sheets API service
            _googleSheets = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Sheets API .NET Quickstart"
            });


        }

        private static string ShowByteValues(byte[] bytes, int last)
        {
            string returnString = "   ";
            for (int ctr = 0; ctr <= last - 1; ctr++)
            {
                if (ctr % 20 == 0)
                    returnString += "\n   ";
                returnString += String.Format("{0:X2} ", bytes[ctr]);
            }
            return returnString;
        }


        // google API quickstart
        public void QuickStart()
        {
            // define request parameters
            string spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            string range = "Class Data!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request = _googleSheets.Spreadsheets.Values.Get(spreadsheetId, range);

            // print names and majors of students in a sample spreadsheet
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Name,     Major");
                foreach (var row in values)
                {
                    // print columns A to E (indecies 0 - 4)
                    Console.WriteLine($"{row[0]},     {row[4]}");
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }
            Console.Read();
        }



    }


    public static class Extentions
    {
        public static byte[] Replace(this byte[] source, byte[] search, byte[] replacement)
        {
            // ReplaceBytes(src, search, repl);
            List<byte> result = new List<byte>();
            var srcLen = source.Length;
            var schLen = search.Length;

            for (int i = 0; i < srcLen; i++)
            {
                // potential match
                if (source[i] == search[0] && i + schLen <= srcLen)
                {
                    var isMatch = true;
                    for (var schIndex = 0; schIndex < schLen; schIndex++)
                    {
                        if (source[schIndex + i] != search[schIndex]) isMatch = false;
                    }
                    // not a match, 
                    if (!isMatch)
                    {
                        result.Add(source[i]);
                        continue;
                    }
                    result.AddRange(replacement);
                    i += schLen - 1;
                    continue;
                }
                result.Add(source[i]);
            }

            return result.ToArray();
        }
        public static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }

        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }
    }
}



