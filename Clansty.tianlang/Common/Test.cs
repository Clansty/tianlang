using MySql.Data.MySqlClient;
using SpreadsheetLight;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Clansty.tianlang
{
    public static class Test
    {
        public static void Do()
        {
            var reply = new Regex(@"\[Reply,.+,SendTime=(\d+).*\]");
            var sample =
                "[Reply,Content=Clansty: 测试一下回复,SendQQID=1980853671,Req=5489,Random=72057595639747560,SendTime=1601795347] b";
            var a = reply.Match(sample);

        }

        static DataTable XlsxToDataTable(string vFilePath)
        {
            var dataTable = new DataTable();
            try
            {
                var sldocument = new SLDocument(vFilePath);
                dataTable.TableName = sldocument.GetSheetNames()[0];
                var worksheetStatistics = sldocument.GetWorksheetStatistics();
                var startColumnIndex = worksheetStatistics.StartColumnIndex;
                var endColumnIndex = worksheetStatistics.EndColumnIndex;
                var startRowIndex = worksheetStatistics.StartRowIndex;
                var endRowIndex = worksheetStatistics.EndRowIndex;
                for (var i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    var cellValueAsRstType = sldocument.GetCellValueAsRstType(1, i);
                    dataTable.Columns.Add(new DataColumn(cellValueAsRstType.GetText(), typeof(string)));
                }

                for (var j = startRowIndex + 1; j <= endRowIndex; j++)
                {
                    var dataRow = dataTable.NewRow();
                    for (var i = startColumnIndex; i <= endColumnIndex; i++)
                    {
                        dataRow[i - 1] = sldocument.GetCellValueAsString(j, i);
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Xlsx to DataTable: \n" + ex.Message);
            }

            return dataTable;
        }
    }
}