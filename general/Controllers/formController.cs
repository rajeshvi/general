using Microsoft.AspNetCore.Mvc;
using general.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace general.Controllers
{
    public class formController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult createtablecolumn(string[] aparenttable, form oform)
        {
            string c = "";
            c += "create table " + oform.table + "(id tinyint identity(1,1),";
            for (int n = 0; n < aparenttable.Length; n++)
                c += aparenttable[n] + "id tinyint,";
            string[] ca = new string[oform.columntypesizes.Count];
            string cj;
            for (int t = 0; t < oform.columntypesizes.Count; t++)
                switch (oform.columntypesizes[t].type)
                {
                    case "text":
                        ca[t] = oform.columntypesizes[t].column + " char(" + oform.columntypesizes[t].size + ")";
                        break;
                    case "number":
                        ca[t] = oform.columntypesizes[t].column + " tinyint";
                        break;
                    case "datetime":
                        ca[t] = oform.columntypesizes[t].column + " datetime";
                        break;
                }
            cj = string.Join(",", ca);
            c += cj + ")";
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            SqlCommand cm1 = new SqlCommand(c, cn);
            cn.Open();
            cm1.ExecuteNonQuery();
            cn.Close();
            List<columntypesize> lcolumntypesize = gettablefields(oform.table, true, true);
            string i = "";
            i += "create procedure add" + oform.table + " ";
            List<string> il3 = new List<string>();
            string is3;
            for (int j = 0; j < lcolumntypesize.Count; j++)
                if (lcolumntypesize[j].column != "id")
                    if (aparenttable.Length > 0)
                        for (int o = 0; o < aparenttable.Length; o++)
                            if (lcolumntypesize[j].column == aparenttable[o] + "id")
                                il3.Add("@" + lcolumntypesize[j].column + " tinyint");
                            else
                                switch (lcolumntypesize[j].type)
                                {
                                    case "text":
                                        il3.Add("@" + lcolumntypesize[j].column + " char(" + lcolumntypesize[j].size + ")");
                                        break;
                                    case "number":
                                        il3.Add("@" + lcolumntypesize[j].column + " tinyint");
                                        break;
                                    case "datetime":
                                        il3.Add("@" + lcolumntypesize[j].column + " datetime");
                                        break;
                                }
                    else
                        switch (lcolumntypesize[j].type)
                        {
                            case "text":
                                il3.Add("@" + lcolumntypesize[j].column + " char(" + lcolumntypesize[j].size + ")");
                                break;
                            case "number":
                                il3.Add("@" + lcolumntypesize[j].column + " tinyint");
                                break;
                            case "datetime":
                                il3.Add("@" + lcolumntypesize[j].column + " datetime");
                                break;
                        }
            string[] ia3 = il3.ToArray();
            is3 = string.Join(",", ia3);
            i += is3;
            i += " as insert into " + oform.table + "(";
            List<string> il1 = new List<string>();
            string is1;
            for (int l = 0; l < lcolumntypesize.Count; l++)
                if (lcolumntypesize[l].column != "id")
                    il1.Add(lcolumntypesize[l].column);
            string[] sa1 = il1.ToArray();
            is1 = string.Join(",", sa1);
            i += is1 + ")values(";
            List<string> il2 = new List<string>();
            string is2;
            for (int k = 0; k < lcolumntypesize.Count; k++)
                if (lcolumntypesize[k].column != "id")
                    il2.Add("@" + lcolumntypesize[k].column);
            string[] sa2 = il2.ToArray();
            is2 = string.Join(",", sa2);
            i += is2 + ")";
            SqlCommand cm3 = new SqlCommand(i, cn);
            cn.Open();
            cm3.ExecuteNonQuery();
            cn.Close();
            string s = "";
            s += "create procedure get" + oform.table + " as select ";
            List<string> sl3 = new List<string>();
            string ss1;
            for (int m = 0; m < lcolumntypesize.Count; m++)
                if (aparenttable.Length > 0)
                {
                    for (int p = 0; p < aparenttable.Length; p++)
                        if (lcolumntypesize[m].column != aparenttable[p] + "id")
                            sl3.Add(lcolumntypesize[m].column);
                }
                else
                    sl3.Add(lcolumntypesize[m].column);
            string[] sa3 = sl3.ToArray();
            ss1 = string.Join(",", sa3);
            s += ss1;
            s += " from " + oform.table;
            SqlCommand cm4 = new SqlCommand(s, cn);
            cn.Open();
            cm4.ExecuteNonQuery();
            cn.Close();
            string u = "";
            u += "create procedure change" + oform.table + " ";
            List<string> ul3 = new List<string>();
            string us3;
            u += "@id tinyint,";
            for (int j = 0; j < lcolumntypesize.Count; j++)
                if (lcolumntypesize[j].column != "id")
                    if (aparenttable.Length > 0)
                    {
                        for (int q = 0; q < aparenttable.Length; q++)
                            if (lcolumntypesize[j].column != aparenttable[q] + "id")
                                switch (lcolumntypesize[j].type)
                                {
                                    case "text":
                                        ul3.Add("@" + lcolumntypesize[j].column + " char("+ lcolumntypesize[j].size + ")");
                                        break;
                                    case "number":
                                        ul3.Add("@" + lcolumntypesize[j].column + " tinyint");
                                        break;
                                    case "datetime":
                                        ul3.Add("@" + lcolumntypesize[j].column + " datetime");
                                        break;
                                }
                    }
                    else
                        switch (lcolumntypesize[j].type)
                        {
                            case "text":
                                ul3.Add("@" + lcolumntypesize[j].column + " char(" + lcolumntypesize[j].size + ")");
                                break;
                            case "number":
                                ul3.Add("@" + lcolumntypesize[j].column + " tinyint");
                                break;
                            case "datetime":
                                ul3.Add("@" + lcolumntypesize[j].column + " datetime");
                                break;
                        }
            string[] ua3 = ul3.ToArray();
            us3 = string.Join(",", ua3);
            u += us3;
            u += " as update " + oform.table + " set ";
            List<string> ul4 = new List<string>();
            string us4;
            for (int l = 0; l < lcolumntypesize.Count; l++)
                if (lcolumntypesize[l].column != "id")
                    if (aparenttable.Length > 0)
                    {
                        for (int r = 0; r < aparenttable.Length; r++)
                            if (lcolumntypesize[l].column != aparenttable[r] + "id")
                                ul4.Add(lcolumntypesize[l].column + "=@" + lcolumntypesize[l].column);
                    }
                    else
                        ul4.Add(lcolumntypesize[l].column + "=@" + lcolumntypesize[l].column);
            string[] sa4 = ul4.ToArray();
            us4 = string.Join(",", sa4);
            u += us4;
            u += " where id=@id";
            SqlCommand cm5 = new SqlCommand(u, cn);
            cn.Open();
            cm5.ExecuteNonQuery();
            cn.Close();
            string d = "";
            d += "create procedure remove" + oform.table + " @id tinyint as delete from " + oform.table;
            SqlCommand cm6 = new SqlCommand(d, cn);
            cn.Open();
            cm6.ExecuteNonQuery();
            cn.Close();
            string co = "create procedure getcount" + oform.table + " as select count(*) from " + oform.table;
            SqlCommand cm7 = new SqlCommand(co, cn);
            cn.Open();
            cm7.ExecuteNonQuery();
            cn.Close();
            string id = "";
            id += "create procedure getid" + oform.table + " @id tinyint as select ";
            List<string> idl1 = new List<string>();
            string ids1;
            for (int m = 0; m < lcolumntypesize.Count; m++)
                if (lcolumntypesize[m].column != "id")
                    if (aparenttable.Length > 0)
                    {
                        for (int v = 0; v < aparenttable.Length; v++)
                            if (lcolumntypesize[v].column != aparenttable[v] + "id")
                                idl1.Add(lcolumntypesize[m].column);
                    }
                    else
                        idl1.Add(lcolumntypesize[m].column);
            string[] ida1 = idl1.ToArray();
            ids1 = string.Join(",", ida1);
            id += ids1;
            id += " from " + oform.table + " where id=@id";
            SqlCommand cm8 = new SqlCommand(id, cn);
            cn.Open();
            cm8.ExecuteNonQuery();
            cn.Close();
            return Json("");
        }

        public List<string> gettablenames()
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            cn.Open();
            SqlCommand cm = new SqlCommand("SELECT name FROM sys.tables", cn);
            SqlDataReader dr = cm.ExecuteReader();
            List<string> ltablenames = new List<string>();
            while (dr.Read())
            {
                ltablenames.Add(dr.GetString(0));
            }
            cn.Close();
            return ltablenames;
        }

        public List<columntypesize> gettablefields(string table, bool id, bool parentid)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            string g = "";
            if (id)
                g = "SELECT column_name as columnname,data_type as datatype FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tablename";
            if (!id)
                g = "SELECT column_name as columnname,data_type as datatype FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tablename and column_name <> 'id' and column_name not like '%id%'";
            if(!id && parentid)
                g = "SELECT column_name as columnname,data_type as datatype FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tablename and column_name <> 'id'";
            if (id && !parentid)
                g = "SELECT column_name as columnname,data_type as datatype FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tablename and (charindex(column_name,'id')=0 or charindex(column_name,'id')=1)";
            SqlCommand cm2 = new SqlCommand(g, cn);
            cm2.Parameters.AddWithValue("@tablename", table);
            List<columntypesize> lcolumntypesize = new List<columntypesize>();
            cn.Open();
            SqlDataReader dr = cm2.ExecuteReader();
            columntypesize ocolumntype;
            int i = 0;
            while (dr.Read())
            {
                ocolumntype = new columntypesize();
                if (id && !parentid)
                {
                    if (dr.GetString(0).IndexOf("id") == 0 || dr.GetString(0).IndexOf("id") == -1)
                    {
                        ocolumntype.column = dr.GetString(0);
                        ocolumntype.type = dr.GetString(1);
                        lcolumntypesize.Add(ocolumntype);
                    }
                }
                else
                {
                    ocolumntype.column = dr.GetString(0);
                    ocolumntype.type = dr.GetString(1);
                    lcolumntypesize.Add(ocolumntype);
                }
                i++;
            }
            cn.Close();
            return lcolumntypesize;
        }

        public List<string> getparents(string table)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            string g = "";
            g = "SELECT column_name as columnname FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tablename and column_name <> 'id' and column_name like '%id%'";
            SqlCommand cm2 = new SqlCommand(g, cn);
            cm2.Parameters.AddWithValue("@tablename", table);
            List<string> lcolumns = new List<string>();
            cn.Open();
            SqlDataReader dr = cm2.ExecuteReader();
            while (dr.Read())
            {
                lcolumns.Add(dr.GetString(0).Substring(0,dr.GetString(0).IndexOf("id")));
            }
            cn.Close();
            return lcolumns;
        }

        public IActionResult addreporttable(string table, string[] aformtable)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            List<columntypesize> lcolumntypesize = gettablefields(table, false, true);
            cn.Open();
            SqlCommand cm = new SqlCommand("add" + table, cn);
            cm.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < lcolumntypesize.Count; i++)
                if (lcolumntypesize[i].column.Contains("id"))
                    cm.Parameters.AddWithValue("@" + lcolumntypesize[i].column, Convert.ToByte(aformtable[i]));
                else
                    cm.Parameters.AddWithValue("@" + lcolumntypesize[i].column, aformtable[i]);
            cm.ExecuteNonQuery();
            cn.Close();
            return Json("");
        }

        public IActionResult getreporttablecount(string table)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            SqlCommand cm = new SqlCommand("getcount" + table, cn);
            cn.Open();
            cm.CommandType = CommandType.StoredProcedure;
            byte b = Convert.ToByte(cm.ExecuteScalar());
            cn.Close();
            return Json(b);
        }

        public List<List<string>> getreporttable(string table)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            SqlCommand cm = new SqlCommand("get" + table + "", cn);
            cn.Open();
            cm.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cm.ExecuteReader();
            List<formtable> lformtable = new List<formtable>();
            List<List<string>> llformtable = new List<List<string>>();
            formtable oformtable;
            while (dr.Read())
            {
                lformtable.Clear();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    oformtable = new formtable();
                    if (dr.GetName(i).IndexOf("id") == 0 || dr.GetName(i).IndexOf("id") == -1)
                        switch (dr.GetDataTypeName(i))
                        {
                            case "tinyint":
                                oformtable.position = i;
                                oformtable.field = Convert.ToString(dr.GetByte(i));
                                break;
                            case "char":
                                oformtable.position = i;
                                oformtable.field = Convert.ToString(dr.GetString(i));
                                break;
                            case "datetime":
                                oformtable.position = i;
                                oformtable.field = Convert.ToString(dr.GetDateTime(i));
                                break;
                        }
                    lformtable.Add(oformtable);
                }
                var dlformtable = (from doformtable in lformtable orderby doformtable.position select doformtable.field).ToList();
                llformtable.Add(dlformtable);
            }
            cn.Close();
            return llformtable;
        }

        public List<string> getidreporttable(string table, byte id)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            SqlCommand cm = new SqlCommand("getid"+ table, cn);
            cn.Open();
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cm.ExecuteReader();
            List<formtable> lformtable = new List<formtable>();
            formtable oformtable;
            while (dr.Read())
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    oformtable = new formtable();
                    oformtable.position = i;
                    oformtable.field = dr.GetString(i);
                    lformtable.Add(oformtable);
                }
            }
            cn.Close();
            var dlformtable = (from doformtable in lformtable orderby doformtable.position select doformtable.field).ToList();
            return dlformtable;
        }

        public IActionResult combinedmultiple(string table, bool oparentid)
        {
            combinedmultiple ocombined = new combinedmultiple();
            ocombined.tablefields = gettablefields(table, true, oparentid);
            ocombined.reporttable = getreporttable(table);
            return Json(ocombined);
        }

        public IActionResult combinedsingle(string table, byte id)
        {
            combinedsingle ocombined = new combinedsingle();
            ocombined.tablefields = gettablefields(table, false, true);
            ocombined.reporttableid = getidreporttable(table, id);
            return Json(ocombined);
        }
        public IActionResult changereporttable(string table, string[] aformtable)
        {
            List<columntypesize> lcolumntype = gettablefields(table, true, true);
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            cn.Open();
            SqlCommand cm = new SqlCommand("change" + table, cn);
            cm.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < lcolumntype.Count; i++)
                cm.Parameters.AddWithValue("@" + lcolumntype[i].column, aformtable[i]);
            cm.ExecuteNonQuery();
            cn.Close();
            return Json("");
        }

        public IActionResult removereporttable(string table, string[] aformtable)
        {
            List<columntypesize> lcolumntype = gettablefields(table, true, true);
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=general;Integrated Security=True;Encrypt=False");
            cn.Open();
            SqlCommand cm = new SqlCommand("remove"+table, cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@id", aformtable[0]);
            cm.ExecuteNonQuery();
            cn.Close();
            return Json("");
        }

        public IActionResult gettablefield()
        {
            List<string> ltablenames = gettablenames();
            report oreport;
            List<columntypesize> lcolumntype;
            List<report> lreport = new List<report>();
            for (int i = 0; i < ltablenames.Count; i++)
            {
                lcolumntype = gettablefields(ltablenames[i], false, true);
                for (int j = 0; j < lcolumntype.Count; j++)
                {
                    oreport = new report();
                    oreport.table = ltablenames[i];
                    oreport.field = lcolumntype[j].column;
                    oreport.type = lcolumntype[j].type;
                    lreport.Add(oreport);
                }
            }
            var results = (from r in lreport group r by r.table into g select new groupreport() { table = g.Key, field = g.Select(m => m.field).ToList(), type = g.Select(m => m.type).ToList() }).ToList();
            return Json(results);
        }

        public IActionResult getresult(string sreport)
        {
            SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=consulting;Integrated Security=True;Encrypt=False");
            cn.Open();
            SqlCommand cm = new SqlCommand(sreport, cn);
            //cm.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cm.ExecuteReader();
            resultformat oresultformat;
            List<resultformat> lresultformat = new List<resultformat>();
            while (dr.Read())
            {
                oresultformat = new resultformat();
                oresultformat.result = dr.GetString(0);
                oresultformat.resultcount = dr.GetInt32(1);
                lresultformat.Add(oresultformat);
            }
            cn.Close();
            return Json(lresultformat);
        }
    }
}
