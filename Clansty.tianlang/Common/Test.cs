using MySql.Data.MySqlClient;
using SpreadsheetLight;
using System;
using System.Data;

namespace Clansty.tianlang
{
    public static class Test
    {
        public static void Do()
        {
            const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = t00rrooT";
            DataTable persons = new DataTable();
            const string sqlPersons = "SELECT * FROM persons";
            var daPersons = new MySqlDataAdapter(sqlPersons, connStr);
            new MySqlCommandBuilder(daPersons);
            daPersons.Fill(persons);
            var dt = XlsxToDataTable(@"C:\Users\clans\Desktop\090310130139Sheet.xlsx");
            //3class 4name 5sex
            foreach (DataRow r in dt.Rows)
            {
                var name = (string)r[4];
                var _class = int.Parse(((string)r[3]).Between("(", ")"));
                var strSex = (string)r[5];
                Sex sex;
                switch (strSex)
                {
                    case "男":
                        sex = Sex.male;
                        break;
                    case "女":
                        sex = Sex.female;
                        break;
                    case "LGBT":
                    case "LGBTQ":
                    case "LGBTQI":
                    case "LGBTQIA":
                    case "LGBTQIAP":
                    case "LGBTQIAPK":
                        sex = Sex.LGBTQIAPK;
                        break;
                    default:
                        sex = Sex.unknown;
                        break;
                }
                //到这里解析的内容已经结束，下面开始写处理代码
                var exist = persons.Select($"name='{r[4]}' AND " +
                    $"sex={(ushort)sex} AND " +
                     "enrollment=2019 AND " +
                     "branch=0");
                if (exist.Length == 1)
                {
                    exist[0]["former_class"] = exist[0]["class"];
                    exist[0]["class"] = _class;
                    continue;
                }
                if (exist.Length > 1)
                {
                    throw new DuplicateNameException(name);
                }
                C.WriteLn($"有新人 {name}");
                //persons.Rows.Add(null, name, 0, 1, 0, (ushort)sex, _class, 2020);
            }
            daPersons.Update(persons);
            Console.WriteLine("数据库保存成功");
        }
        static DataTable XlsxToDataTable(string vFilePath)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SLDocument sldocument = new SLDocument(vFilePath);
                dataTable.TableName = sldocument.GetSheetNames()[0];
                SLWorksheetStatistics worksheetStatistics = sldocument.GetWorksheetStatistics();
                int startColumnIndex = worksheetStatistics.StartColumnIndex;
                int endColumnIndex = worksheetStatistics.EndColumnIndex;
                int startRowIndex = worksheetStatistics.StartRowIndex;
                int endRowIndex = worksheetStatistics.EndRowIndex;
                for (int i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    SLRstType cellValueAsRstType = sldocument.GetCellValueAsRstType(1, i);
                    dataTable.Columns.Add(new DataColumn(cellValueAsRstType.GetText(), typeof(string)));
                }
                for (int j = startRowIndex + 1; j <= endRowIndex; j++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = startColumnIndex; i <= endColumnIndex; i++)
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
