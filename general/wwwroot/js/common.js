$().ready(function () {
    aid = [];
    $("#buttons").css("display", "none");
    $("#tgenerate").html("<tr><td><input type='text'></td><td></td><td></td><td></td></tr>");
    for (i = 0; i < 10; i++)
        $("#tgenerate").append("<tr><td></td><td><input type='text'></td><td><select><option>text</option><option>number</option><option>datetime</option></select></td><td><input type='number'/></td></tr>");
    tablenames = [];

    $("#bgenerate").click(function () {
        getmenu();
        gettables();
        form = {};
        form.columntypes = [];
        $("#tgenerate tr").each(function (index) {
            columntypesize = {};
            if ($(this).find("td:eq(0)").html() != "")
                if ($(this).find("td:eq(0) input").val() != "")
                    form.table = $(this).find("td:eq(0) input").val();
            if ($(this).find("td:eq(1)").html() != "")
                if ($(this).find("td:eq(1) input").val() != "")
                {
                    columntypesize.column = $(this).find("td:eq(1) input").val();
                    columntypesize.type = $(this).find("td:eq(2) select").val();
                    columntypesize.size = $(this).find("td:eq(3) input").val();
                    form.columntypes.push(columntype);
                }
        });
        $.post("../form/createtablecolumn", { aparenttable: tablenames, oform: form }, function (result) {
            alert(result);
        });
    });
    
    function getmenu()
    {
        $.get("../form/gettablenames", function (data) {
            s = "";
            s += "<tr>";
            s += "<td>generate</td>";
            for (i = 0; i < data.length; i++)
                s += "<td>" + data[i] + "</td>";
            s += "</tr>";
            $("#menu").html("");
            $("#menu").html(s);
        });
    }

    getmenu();

    function gettables()
    {
        $.get("../form/gettablenames", function (data) {
            s = "";
            for (i = 0; i < data.length; i++)
                s += "<tr><td>" + data[i] + "</td></tr>";
            $("#tables").html("");
            $("#tables").html(s);
        });
    }

    gettables();

    $("#menu").on("click", "td", function () {
        if ($(this).text() != "generate")
        {
            $("#tgenerate").css("display", "none");
            $("#bgenerate").css("display", "none");
            $("#tables").css("display", "none");
            $("#buttons").css("display", "block");
            $("#buttons td").css("width", $("#single").css("width"));
        }
        $.post("../form/gettablefields", { table: $(this).text(), id: false, parent: true }, function (data) {
            s = "";
            for (i = 0; i < data.length; i++)
            {
                s += "<tr><td>" + data[i].column + "</td><td><input type='text'/></td></tr>";
            }
            $("#single").html(s);
        });
        $.post("../form/getparents", { table: $(this).text() }, function (data) {
            for (i = 0; i < data.length; i++)
                getparenttable(data[i]);
        });
        alert()
        getreporttable($(this).text());
        tablename = $(this).text();
    });

    $("#tables").on("click", "td", function () {
        tablenames.push($(this).text());
    });

    function getparenttable(parenttable) {
        $.post("../form/getreporttablecount", { table: parenttable }, function (data) {
            if (data > 0)
            {
                parentid = false;
                $.post("../form/combinedmultiple", { table: parenttable, oparentid: parentid }, function (data) {
                    s = "";
                    s += "<table>";
                    s += "<tr>";
                    for (i = 0; i < data.tablefields.length; i++)
                        s += "<td>" + data.tablefields[i].column + "</td>";
                    s += "</tr>";
                    for (i = 0; i < data.reporttable.length; i++) {
                        s += "<tr>";
                        for (j = 0; j < data.reporttable[i].length; j++)
                            s += "<td>" + data.reporttable[i][j] + "</td>";
                        s += "</tr>";
                    }
                    s += "</table>";
                    /*$("#multiple td").css("width", $("#single").css("width"));*/
                    $("#parents").html(s);
                    $("#parents table").css("width", "99%");
                    $("#parents table").css("border-collapse", "collapse");
                    $("#parents td").css("border","1px solid");
                });
            }
        });
    }
    
    $("#parents").on("click", "tr", function () {
        $(this).find("td").toggleClass("selected");
        aid.push($(this).find("td:eq(0)").text());
    });
    
    $("#insert").click(function () {
        formtable = [];
        for (i = 0; i < aid.length; i++)
            formtable.push(aid[i]);
        $("#single input").each(function () {
            formtable.push($(this).val());
        });
        
        $.post("../form/addreporttable", { table: tablename, aformtable: formtable }, function (data) {
            alert(data);
        });
    });
    
    function getreporttable(tablename)
    {
        $.post("../form/getreporttablecount", { table: tablename }, function (data) {
            if (data > 0)
                parentid = true;
                $.post("../form/combinedmultiple", { table: tablename, operentid: parentid }, function (data) {
                    s = "";
                    s += "<tr>";
                    for (i = 0; i < data.tablefields.length; i++)
                        s += "<td>" + data.tablefields[i].column + "</td>";
                    s += "</tr>";
                    for (i = 0; i < data.reporttable.length; i++) {
                        s += "<tr>";
                        for (j = 0; j < data.reporttable[i].length; j++)
                            s += "<td>" + data.reporttable[i][j] + "</td>";
                        s += "</tr>";
                    }
                    $("#multiple").html(s);
                    /*$("#multiple td").css("width", $("#single").css("width"));*/
                });
        });
    }
    
    $("#multiple").on("click", "tr", function () {
        $(this).find("td").toggleClass("selected");
        id = $(this).find("td:eq(0)").text();
        $("#single").html("");
        $.post("../form/combinedsingle", { table: tablename, id: $(this).find("td:eq(0)").text() }, function (data) {
            s = "";
            for (i = 0; i < data.tablefields.length; i++)
                s += "<tr><td>" + data.tablefields[i].column + "</td><td><input type='text' value=" + data.reporttableid[i] + "/></td></tr>";
            $("#single").html(s);
        });
    });
    
    $("#update").click(function () {
        formtable = [];
        formtable.push(id);
        $("#single input").each(function () {
            formtable.push($(this).val());
        });
        $.post("../form/changereporttable", { table: tablename, aformtable: formtable }, function (data) {
            alert(data);
        });
    });
    
    $("#delete").click(function () {
        formtable = [];
        formtable.push(id);
        $("#single input").each(function () {
            formtable.push($(this).val());
        });
        $.post("../form/removereporttable", { table: tablename, aformtable: formtable }, function (data) {
            alert(data);
        });
    });
    
    $.get("../form/gettablefield", function (data) {
        s = "";
        for (i = 0; i < data.length; i++) {
            s += "<tr>";
            s += "<td>" + data[i].table + "</td><td></td><td></td>";
            for (j = 0; j < data[i].field.length; j++) {
                s += "<tr>";
                s += "<td></td><td>" + data[i].field[j] + "</td><td>" + data[i].type[j] + "</td>";
                s += "</tr>";
            }
            s += "</tr>";
        }
        $("#tablecolumns").append(s);
    });

    tablecolumns = [];
    
    $("#tablecolumns").on("click", "tr", function () {
        tablecolumn = {};
        tablecolumn.columnname = $(this).find("td:eq(1)").text();
        tablecolumn.type = $(this).closest("tr").find("td:eq(2)").text();
        r = $(this).closest("tr");
        do {
            r = r.prev();
            //alert(r.index());
            tablecolumn.tablename = r.find("td:eq(0)").text();
        }
        while (r.find("td:eq(0)").text() == "");
        tablecolumns.push(tablecolumn);
    });

    $("#getdata").click(function (e) {
        e.preventDefault();
        s = "";
        for (i = 0; i < tablecolumns.length; i++) {
            s += "<tr>";
            s += "<th>" + tablecolumns[i].columnname + "</th>";
            s += "</tr>";
            s += "<tr>";
            if (tablecolumns[i].type == "char")
                s += "<td><input type='text' id='character" + parseInt(i + 1) + "'/></td>";
            else
                s += "<td>&gt;=<input type='text' id='lessthan" + parseInt(i + 1) + "'/>=<input type='text' id='equalto" + i + "'/>&lt;=<input type='text' id='greaterthan" + i + "'/></td>";
            s += "</tr>";
            s += "<tr>";
            s += "<td>";
            s += "<select id='groupby" + i + "'>";
            s += "<option>sum</option>";
            s += "<option>count</option>";
            s += "<option>avg</option>";
            s += "<option>min</option>";
            s += "<option>max</option>";
            s += "</select>";
            s += "</td>";
            s += "</tr>";
        }
        $("#query").html(s);
        return false;
    });

    $("#getresult").click(function (e) {
        e.preventDefault();
        alert()
        character1 = $("#character1").val();
        character2 = $("#character2").val();
        lessthan1 = $("#lessthan1").val();
        lessthan2 = $("#lessthan2").val();
        greaterthan1 = $("#greaterthan1").val();
        greaterthan2 = $("#greaterthan2").val();
        equalto1 = $("#equalto1").val();
        equalto2 = $("#equalto2").val();
        groupby1 = $("#groupby1").val();
        groupby2 = $("#groupby2").val();
        if (tablecolumns[0].tablename == tablecolumns[1].tablename) {
            report = "";
            report += "select " + tablecolumns[0].columnname + "," + groupby1 + "(" + tablecolumns[1].columnname + ") from " + tablecolumns[0].tablename + " where ";
            
            if (tablecolumns[0].type == "char")
                if (character1 != "")
                    report += tablecolumns[0].columnname + " like '%" + character1 + "%' ";
                else {
                    if (equalto1 != "")
                        report += tablecolumns[0].columnname + "=" + character1;
                    if (lessthan1 != "" && greaterthan1 != "")
                        report += tablecolumns[0].columnname + ">=" + greaterthan1 && tablecolumns[0].columnname + "<=" + lessthan1;
                }
            report += " and ";
            
            if (tablecolumns[1].type == "char")
                if (character2 != "")
                    report += tablecolumns[1].columnname + " like '%" + character2 + "%'";
                else {
                    if (equalto2 != "")
                        report += tablecolumns[1].columnname + "=" + character2;
                    if (lessthan2 != "" && greaterthan2 != "")
                        report += tablecolumns[1].columnname + ">=" + greaterthan2 && tablecolumns[1].columnname + "<=" + lessthan2;
                }
            
            report += " group by " + tablecolumns[0].columnname;
            
            $.post("../form/getresult", { sreport: report }, function (data) {
                if (data.length > 0) {
                    s = "";
                    for (i = 0; i < data.length; i++) {
                        s += "<tr>";
                        s += "<td>" + data[i].result + "</td>";
                        s += "<td>" + data[i].resultcount + "</td>";
                        s += "</tr>";
                    }
                    $("#result").append(s);
                }
            });
        }
        return false;
    });
});