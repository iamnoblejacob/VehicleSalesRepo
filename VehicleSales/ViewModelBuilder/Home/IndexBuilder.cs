using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using VehicleSales.Models;

namespace VehicleSales.ViewModelBuilder.Home
{
    public class IndexBuilder
    {
        private HttpPostedFileBase FileUpload;

        public IndexBuilder(HttpPostedFileBase fileUpload)
        {
            this.FileUpload = fileUpload;
        }

        // Build the Index view parsing the csv file
        internal void Build(ModelStateDictionary modelState, VehicleSalesModel viewModel)
        {
            if (modelState.IsValid == false)
            {
                return;
            }

            if (FileUpload != null && FileUpload.ContentLength > 0)
            {
                if (FileUpload.FileName.EndsWith(".csv"))
                {
                    // read file data to string
                    string csvData = StreamToString(FileUpload.InputStream);

                    if(string.IsNullOrEmpty(csvData))
                    {
                        modelState.AddModelError("File", "File is empty");
                        return;
                    }

                    // read csv to model
                    viewModel.SalesList = ReadCSVFile(csvData);

                    if(viewModel.SalesList == null || viewModel.SalesList.Count() <= 0)
                    {
                        modelState.AddModelError("File", "File error");
                        return;
                    }

                    // get vehicle sent most often
                    viewModel.MostSoldVehicle = GetMostSoldVehicle(viewModel.SalesList);
                }
                else
                {
                    modelState.AddModelError("File", "This file format is not supported");
                }
            }
            else
            {
                modelState.AddModelError("File", "Please Upload Your file");
            }
        }

        // Get the most sold vehicle
        private string GetMostSoldVehicle(List<SalesModel> salesList)
        {
            string mostSoldVehicle = string.Empty;

            var mostSoldRecord = salesList.GroupBy(s=>s.Vehicle).OrderByDescending(s=>s.Count()).FirstOrDefault();
            mostSoldVehicle = mostSoldRecord.Key;

            return mostSoldVehicle;
        }

        // Read the csv file to model
        private List<SalesModel> ReadCSVFile(string csvData)
        {
            List<SalesModel> returnData = new List<SalesModel>();

            try
            {
                bool skippedLine1 = false;
                // read data to model
                foreach (string row in csvData.Split('\n'))
                {
                    // skipe the header
                    if (skippedLine1 == false)
                    {
                        skippedLine1 = true;
                        continue;
                    }

                    // check if row has comma
                    if (row.Contains(',') == false)
                    {
                        return returnData;
                    }

                    if (!string.IsNullOrEmpty(row))
                    {
                        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        String[] columnData = CSVParser.Split(row);

                        int.TryParse(columnData[0], out int dealNumber);
                        decimal.TryParse(columnData[4].Replace(",","").Replace("\"", ""), out decimal price);
                        DateTime.TryParse(columnData[5], out DateTime date);

                        returnData.Add(new SalesModel
                        {
                            CustomerName = columnData[1],
                            DealershipName = columnData[2],
                            Vehicle = columnData[3],
                            Date = date,
                            DealNumber = dealNumber,
                            Price = price
                        });
                    }

                }
            }
            catch (Exception)
            {
                // any failure will not show any data
                returnData = null;
            }

            return returnData;
        }

        // Get the string data from stream
        private string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}