

//var script = document.createElement('script');
//script.src = '/src/app/assets/scripts/jquery.blockUI.js';
//document.head.appendChild(script);

function setButtonOptions(a) {
  thisLow = null;
  thisHigh = null;
  if (chartData == "fix") {
    Highcharts.setOptions({
      rangeSelector: {
        buttons: [{
          type: "month",
          count: 1,
          text: "1m"
        }, {
          type: "month",
          count: 2,
          text: "2m"
        }, {
          type: "month",
          count: 3,
          text: "3m"
        }, {
          type: "month",
          count: 6,
          text: "6m"
        }, {
          type: "month",
          count: 9,
          text: "9m"
        }, {
          type: "year",
          count: 1,
          text: "1y"
        }, {
          type: "all",
          text: "All"
        }]
      }
    })
  } else {
    Highcharts.setOptions({
      rangeSelector: {
        buttons: [{
          type: "minute",
          count: 60 * 24,
          text: "1d"
        }, {
          type: "minute",
          count: 60 * 120,
          text: "5d"
        }, {
          type: "month",
          count: 1,
          text: "1m"
        }, {
          type: "month",
          count: 3,
          text: "3m"
        }, {
          type: "month",
          count: 6,
          text: "6m"
        }, {
          type: "year",
          count: 1,
          text: "1y"
        }, {
          type: "all",
          text: "All"
        }]
      }
    })
  }
}
centerBlock = false;
var chart;
var metalName = "Gold";
var chartType = "day";
var chartData = "spot";
var lastClickEvent = Number(new Date);
var startDate = Date.today().add(-12).months().toString("MM/dd/yyyy");
var endDate = Date.today().toString("MM/dd/yyyy");
var maxEndDate = Date.today().toString("MM/dd/yyyy");
var minStartDate = "1/1/2001";
var lowExtreme = Number(new Date(startDate));
var highExtreme = Number(new Date(endDate));
var allData = [];
var newStartDate = new Date(startDate);
var newEndDate = new Date(endDate);
var span = (new TimeSpan(newEndDate - newStartDate)).getDays();
var axisRange = span;
var forceRedraw = false;
var tableHeader;
var tableData;
var chartDataLabel = "Spot Prices";
var chartDayPointLabel = "Bid Close";
var chartPointTimeAmPm = "";
var toolCloseText = " Bid Close";
var curDataRow = "";
var thisHigh;
var thisLow;
var comCode = "G";

function loadHighStock1() {
  debugger;
  window.chart = new Highcharts.StockChart({
    chart: {
      renderTo: 'container'
    },

    rangeSelector: {
      selected: 1
    },

    title: {
      text: 'Example'
    },

    series: [{
      name: 'Data',
      data: [1, 2, 3, 4, 3, 1],
      tooltip: {
        valueDecimals: 2
      }
    }]
  })
}

function setViewType() {
  
    var comCode = "G";
  jQuery_3_3_1("button").button();

  jQuery_3_3_1("#view-type").buttonset();
    $("#view-type-chart").click(function () {$("#containerdata").css("display", "none"); $("#container").css("display", ""); });
    $("#view-type-data").click(function () {$("#container").css("display", "none"); $("#containerdata").css("display", ""); });



}

function loadHighStock() {
  setViewType();
  function f() {
    if (chartType == "day") {
      var a = "dddd, MMMM dd, yyyy"
    } else {
      var a = "dddd, MMMM dd, yyyy h:mm tt"
    }
    chart = new Highcharts.StockChart({
      chart: {
        renderTo: "container",
        plotShadow: false,
        plotBorderWidth: 1,
        plotBorderColor: "#9bb5d0",
        backgroundColor: {
          linearGradient: [0, 0, 0, 1500],
          stops: [
            [0, "rgb(255, 255, 255)"],
            [1, "rgb(144, 167, 200)"]
          ]
        },
        borderColor: "#ccc",
        spacingBottom: 10,
        events: {
          redraw: function () {
            if (chart) {
              lowExtreme = parseInt(this.xAxis[0].getExtremes().min);
              highExtreme = parseInt(this.xAxis[0].getExtremes().max)
            }
            span = (new TimeSpan(highExtreme - lowExtreme)).getDays();
            endDate = (new Date(highExtreme)).toString("MM/dd/yyyy");
            newEndDate = new Date(highExtreme);
            this.setTitle({
              style: {
                color: "#3E576F",
                fontSize: "14px",
                fontWeight: "normal"
              },
              text: getMetalNameFromComCode(comCode) + " " + chartDataLabel + " from " + (new Date(lowExtreme)).toString("MMMM dd, yyyy") + " to " + (new Date(highExtreme)).toString("MMMM dd, yyyy") + " <br/> High: <b>$" + addCommas(thisHigh) + "</b>        Low: <b>$" + addCommas(thisLow) + "</b>"
            });
            axisRange = parseInt((highExtreme - lowExtreme) / 1e3 / 3600 / 24);
            if (axisRange == 0) {
              axisRange = 1
            }
            c(axisRange, false, (new Date(lowExtreme)).toString("MM/dd/yyyy"), (new Date(highExtreme)).toString("MM/dd/yyyy"))
          }
        }
      },
      navigator: {
        maskFill: "rgba(255, 255, 255, 0.75)",
        xAxis: {
          dateTimeLabelFormats: {
            second: "%H:%M:%S",
            minute: "%H:%M",
            hour: "%H:%M",
            day: "%b %d",
            week: "%b %d",
            month: "%b '%y",
            year: "%Y"
          }
        }
      },
      plotOptions: {
        area: {
          fillColor: null,
          fillOpacity: .05
        }
      },
      tooltip: {
        formatter: function () {
          if (chartData == "fix") {
            toolCloseText = " PM Fix";
            chartPointTimeAmPm = ""
          } else {
            if (chartType == "day") {
              toolCloseText = " Bid Close";
              chartPointTimeAmPm = ""
            } else {
              toolCloseText = " Bid";
              chartPointTimeAmPm = " PT"
            }
          }
          var b = (new Date(this.x)).toString(a) + chartPointTimeAmPm;
          $.each(this.points, function (a, c) {
            b += "<br/><b>" + getMetalNameFromComCode(comCode) + toolCloseText + ": $" + addCommas(c.y.toFixed(2)) + " USD</b>"
          });
          return b
        }
      },
      zoomType: "full",
      credits: {
        enabled: false
      },
      title: {
        text: getMetalNameFromComCode(comCode) + " " + chartDataLabel + " from " + (new Date(lowExtreme)).toString("dddd, MMMM dd, yyyy") + " to " + (new Date(highExtreme)).toString("dddd, MMMM dd, yyyy")
      },
      yAxis: {
        opposite: true,
        gridLineColor: "#ddd",
        minorGridLineColor: "#f7f7f7",
        minorGridLineWidth: 1,
        lineWidth: 0,
        tickLength: 0,
        labels: {
          align: "left",
          x: 8,
          y: 0
        }
      },
      xAxis: {
        maxZoom: 1e3 * 60 * 60,
        gridLineColor: "#eee",
        tickLength: 0,
        lineWidth: 0,
        dateTimeLabelFormats: {
          second: "%H:%M:%S",
          minute: "%H:%M",
          hour: "%H:%M",
          day: "%b %d",
          week: "%b %d",
          month: "%b '%y",
          year: "%Y"
        }
      },
      rangeSelector: {
        inputEnabled: false,
        buttonTheme: {
          style: {}
        }
      },
      series: [{
        cropThreshold: 5,
        name: "Spot Prices",
        type: "area",
        data:allData,
        //data: [[1298851200000, 100768479],

        //  [1298937600000, 114032163],

        //  [1299024000000, 150647189],

        //  [1299110400000, 125196764],

        //  [1299196800000, 113316483],

        //  [1299456000000, 136530149],

        //  [1299542400000, 89078955],

        //  [1299628800000, 113349033],

        //  [1299715200000, 126972055],

        //  [1299801600000, 117770016]],
        threshold: null
      }]
    }, function (a) {
      a.renderer.text("www.amark.com", 775, 400).attr({
        zIndex: 4
      }).css({
        color: "#777",
        fontSize: "14px",
        fontWeight: "bold"
      }).add();
      this.setTitle({
        style: {
          color: "#3E576F",
          fontSize: "14px",
          fontWeight: "normal"
        },
        text: getMetalNameFromComCode(comCode) + " " + chartDataLabel + " from " + (new Date(lowExtreme)).toString("MMMM dd, yyyy") + " to " + (new Date(highExtreme)).toString("MMMM dd, yyyy") + " <br/> High: <b>$" + addCommas(thisHigh) + "</b>        Low: <b>$" + addCommas(thisLow) + "</b>"
      })
    }, function (a) {
      a.xAxis[0].setExtremes([lowExtreme], [highExtreme])
    })
  }

  function e() {
    $("#container").block({
      message: "<span style='font-weight:bold;font-size:20px;color:#fff'><img style='vertical-align:text-top;margin-right:10px;' src='~/src/app/assets/images/ajax-loader.gif'>Please wait...</span>",
      css: {
        border: "none",
        padding: "15px",
        backgroundColor: "#000",
        "-webkit-border-radius": "10px",
        "-moz-border-radius": "10px",
        opacity: .5,
        color: "#fff"
      }
    });
    $("#tableContainer").block({
      message: "<span style='font-weight:bold;font-size:20px;color:#fff'><img style='vertical-align:text-top;margin-right:10px;' src='/content/images/ajax-loader.gif'>Please wait...</span>",
      css: {
        border: "none",
        padding: "15px",
        backgroundColor: "#000",
        "-webkit-border-radius": "10px",
        "-moz-border-radius": "10px",
        opacity: .5,
        color: "#fff"
      }
    });
    if (chartData == "fix") {
      var a = "api/feeds/fixhistory"
    } else {
      var a = "api/feeds/spothistory"
    }
    $.post(a, {
      comCode: comCode,
      startDate: startDate,
      endDate: endDate,
      groupBy: chartType
    }, function (a) {
      debugger;
      var b;
      var c;
      tableData = "";
      if (chartData == "fix") {
        if (comCode == "S") {
          tableHeader = "<th>Date</th><th>Silver Fix</th>"
        } else {
          tableHeader = "<th>Date</th><th>AM Fix</th><th>PM Fix</th>"
        }
      } else {
        if (chartType == "min") {
          tableHeader = "<th>Date and Time</th><th>Bid</th><th>Ask</th>"
        } else {
          tableHeader = "<th>Date</th><th>Open</th><th>High</th><th>Low</th><th>Close</th>"
        }
      }
      $.each(a, function (a, d) {
        debugger;
        //b = new Date(parseInt(d.Update_Date.substr(6)));
        //b = Number(b);
        b = new Date(d.Update_Date).getTime();
        if (chartData == "fix") {
          c = parseFloat(d.Bid).toFixed(2);
          if (!thisLow || parseFloat(d.Bid).toFixed(2) < parseFloat(thisLow)) {
            thisLow = c
          }
          if (!thisHigh || parseFloat(d.Bid).toFixed(2) > parseFloat(thisHigh)) {
            thisHigh = c
          }
          curDataRow = "<tr><td>" + (new Date(d.Update_Date.substr(6))).toString("MMM dd, yyyy") + "</td><td>$" + addCommas(parseFloat(d.Bid).toFixed(2)) + "</td>";
          if (comCode != "S") {
            curDataRow += "<td>$" + addCommas(parseFloat(d.Ask).toFixed(2)) + "</td>"
          }
          curDataRow += "</tr>";
          tableData = curDataRow + tableData
        } else {
          if (chartType == "min") {
            c = d.Bid;
            if (!thisLow || d.Bid < thisLow) {
              thisLow = d.Bid.toFixed(2)
            }
            if (!thisHigh || d.Bid > thisHigh) {
              thisHigh = d.Bid.toFixed(2)
            }
            tableData = "<tr><td>" + (new Date(d.Update_Date.substr(6))).toString("MMM dd, yyyy h:mm tt") + "</td><td>$" + addCommas(d.Bid.toFixed(2)) + "</td><td>$" + addCommas(d.Ask.toFixed(2)) + "</td></tr>" + tableData
          } else {
            c = d.Bid_Close;
            if (!thisLow || d.Bid_Close < thisLow) {
              thisLow = d.Bid_Close.toFixed(2)
            }
            if (!thisHigh || d.Bid_Close > thisHigh) {
              thisHigh = d.Bid_Close.toFixed(2)
            }
            tableData = "<tr><td>" + (new Date(d.Update_Date.substr(6))).toString("MMM dd, yyyy") + "</td><td>$" + addCommas(d.Bid_Open.toFixed(2)) + "</td><td>$" + addCommas(d.Bid_High.toFixed(2)) + "</td><td>$" + addCommas(d.Bid_Low.toFixed(2)) + "</td><td>$" + addCommas(d.Bid_Close.toFixed(2)) + "</td></tr>" + tableData
          }
        }
        allData.push([b, parseFloat(c)])
      })
    }, "json").fail(function () {
      $("#container").unblock();
      $("#tableContainer").unblock();
      alert("There was an error loading the data you requested.  Please ensure you've entered valid date values for the search.")
    }).done(function () {
      $("#container").unblock();
      $("#tableContainer").unblock();
      if (allData.length == 0) {
        alert("Your search did not return any records.    Please ensure you've entered valid date values for the search.  NOTE:  Weekends will not contain data.")
      } else {
        if (chart) {
          chart.destroy()
        }
        f();
        jQuery_3_3_1("#gridTitleStart").html(jQuery_3_3_1("#startdate").val());
        jQuery_3_3_1("#gridTitleEnd").html(jQuery_3_3_1("#enddate").val());
        jQuery_3_3_1("#myTable>thead>tr").empty();
        jQuery_3_3_1("#myTable>tbody").empty();
        jQuery_3_3_1(tableHeader).appendTo("#myTable>thead>tr");
        jQuery_3_3_1(tableData).appendTo("#myTable>tbody");

        function a(a) {
          var b = new Date(a);
          if (b.toString() == "NaN" || b.toString() == "Invalid Date") {
            return false
          } else {
            return true
          }
        }
        var b = function (b) {
          var c = b.innerHTML;
          if (!a(c)) {
            c = c.replace("$", "");
            c = c.replace(/,/g, "");
            c = parseFloat(c);
            return c
          } else {
            if (c.substring(c.length - 1) != "M") {
              return c + " 12:00 AM"
            } else {
              return c
            }
          }
        };
        $("#myTable").tablesorter({
          textExtraction: b
        })
      }
    })
  }

  function d() {
    lastClickEvent = Number(new Date);
    startDate = jQuery_3_3_1("#startdate").val();
    endDate = jQuery_3_3_1("#enddate").val();
    var a = new Date(startDate);
    var b = new Date(endDate);
    if ((new Date(endDate)).isAfter(new Date(maxEndDate))) {
      jQuery_3_3_1("#enddate").val(maxEndDate);
      endDate = maxEndDate;
      b = new Date(maxEndDate)
    }
    if ((new Date(startDate)).isBefore(new Date(minStartDate))) {
      alert("Data is only available dating back to " + minStartDate + ".  Your FROM date has been adusted accordingly.");
      jQuery_3_3_1("#startdate").val(minStartDate);
      startDate = minStartDate;
      a = new Date(minStartDate)
    }
    span = (new TimeSpan(b - a)).getDays();
    if (span >= 0) {
      allData.length = 0;
      lowExtreme = Number(a);
      highExtreme = Number(b) + 1e3 * 3600 * 24 - 1;
      axisRange = span;
      c(span, false)
    }
  }

  function c(a, b, c, d) {
    debugger;
    var f = false;
    thisHigh = null;
    thisLow = null;
    if (a < 10 && chartData == "spot") {
      var g = "min"
    } else {
      var g = "day"
    }
    if (chartType != g && chartData != "fix") {
      chartType = g;
      if (chartType == "min") {
        startDate = (new Date(lowExtreme)).toString("MM/dd/yyyy");
        endDate = (new Date(highExtreme)).toString("MM/dd/yyyy")
      } else {
        lowExtreme = Number(new Date(startDate));
        highExtreme = Number(new Date(endDate)) + 1e3 * 3600 * 24 - 1
      }
      f = true
    }
    if (!b || forceRedraw || b && f) {
      allData.length = 0;
      startDate = c == null ? startDate : c.toString("MM/dd/yyyy");
      endDate = d == null ? endDate : d.toString("MM/dd/yyyy");
      jQuery_3_3_1("#startdate").val(startDate);
      jQuery_3_3_1("#enddate").val(endDate);
      e();
      setButtonOptions(a);
      forceRedraw = false
    }
  }

  function b(a) {
    thisHigh = null;
    thisLow = null;
    if (comCode != a) {
      comCode = a;
      allData.length = 0;
      e()
    }
  }
  Highcharts.setOptions({
    lang: {
      rangeSelectorZoom: "Zoom:"
    }
  });
  Highcharts.setOptions({
    lang: {
      rangeSelectorFrom: "Zoom Current Data From:"
    }
  });
  c(span, false);
  jQuery_3_3_1("#refreshchart").click(function () {
    if ((new Date(endDate)).isBefore(new Date(startDate))) {
      alert("Please select a FROM date that is earlier than the TO date.")
    } else {
      if (Number(new Date) - lastClickEvent > 2e3) {
        d()
      }
    }
  });
  jQuery_3_3_1("#switchChartData").click(function () {
    if (chartData == "spot") {
      chartData = "fix";
      jQuery_3_3_1("#switchChartData").html("Switch to Spot Prices");
      chartDataLabel = "European Fixes";
      chartDayPointLabel = "PM Fix"
    } else {
      chartData = "spot";
      jQuery_3_3_1("#switchChartData").html("Switch to European Fixes");
      chartDataLabel = "Spot Prices";
      chartDayPointLabel = "Bid Close"
    }
    jQuery_3_3_1("#title-chart-data").html(chartDataLabel);
    if (chartType == "min") {
      span = 365;
      chartType = "day";
      highExtreme = Number(new Date(maxEndDate));
      jQuery_3_3_1("#startdate").val((new Date(highExtreme)).add(-span).days().toString("MM/dd/yyyy"));
      startDate = jQuery_3_3_1("#startdate").val();
      newStartDate = new Date(startDate);
      lowExtreme = Number(new Date(startDate));
      jQuery_3_3_1("#enddate").val((new Date(highExtreme)).toString("MM/dd/yyyy"));
      endDate = jQuery_3_3_1("#enddate").val();
      newEndDate = new Date(endDate)
    }
    c(span, false)
  });
  jQuery_3_3_1("#startdate").val(startDate);
  jQuery_3_3_1("#enddate").val(endDate);
  jQuery_3_3_1("#startdate, #enddate").datepicker().change(function () {
    d()
  });
  jQuery_3_3_1("button").button();
  jQuery_3_3_1("#chart-spot").buttonset();
  jQuery_3_3_1("#chart-spot-gold").click(function () {
    b("G")
  });
  jQuery_3_3_1("#chart-spot-silver").click(function () {
    b("S")
  });
  jQuery_3_3_1("#chart-spot-platinum").click(function () {
    b("P")
  });
  jQuery_3_3_1("#chart-spot-palladium").click(function () {
    b("L")
  });
  var a = jQuery_3_3_1("#chart-spot-" + getMetalNameFromComCode(comCode).toLowerCase());
  a[0].checked = true;
  a.button("refresh");
  window.manualRedraw = function (a) {
    span = a;
    jQuery_3_3_1("#startdate").val((new Date(highExtreme)).add(-span).days().toString("MM/dd/yyyy"));
    startDate = jQuery_3_3_1("#startdate").val();
    newStartDate = new Date(startDate);
    lowExtreme = Number(new Date(startDate));
    jQuery_3_3_1("#enddate").val((new Date(highExtreme)).toString("MM/dd/yyyy"));
    endDate = jQuery_3_3_1("#enddate").val();
    newEndDate = new Date(endDate);
    highExtreme = Number(new Date(endDate));
    forceRedraw = true;
    c(a, false)
  }
}

function getMetalNameFromComCode(comCode) {
  switch (comCode) {
    case "G":
      return "Gold";
      break;
    case "S":
      return "Silver";
      break;
    case "P":
      return "Platinum";
      break;
    case "L":
      return "Palladium";
      break;
  }
}

function addCommas(nStr) {
  nStr += '';
  x = nStr.split('.');
  x1 = x[0];
  x2 = x.length > 1 ? '.' + x[1] : '';
  var rgx = /(\d+)(\d{3})/;
  while (rgx.test(x1)) {
    x1 = x1.replace(rgx, '$1' + ',' + '$2');
  }
  return x1 + x2;
}
