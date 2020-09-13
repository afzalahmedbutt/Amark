(function () {
  debugger;
  window.jQuery = jQuery_1_6_2;
  window.$ = jQuery_1_6_2;
  function cy(k, l) {
    function dg() {
      var a = "onreadystatechange",
        d = "complete";
      if (!u && c == c.top && b.readyState !== d) {
        b.attachEvent(a, function () {
          b.detachEvent(a, dg);
          if (b.readyState === d) {
            dg()
          }
        });
        return
      }
      da();
      bA(bo, "init");
      if (Highcharts.RangeSelector && k.rangeSelector.enabled) {
        bo.rangeSelector = new Highcharts.RangeSelector(bo)
      }
      cG();
      cH();
      dd();
      cX();
      bt(k.series || [], function (a) {
        cQ(a)
      });
      if (Highcharts.Scroller && (k.navigator.enabled || k.scrollbar.enabled)) {
        bo.scroller = new Highcharts.Scroller(bo)
      }
      bo.render = de;
      bo.tracker = cf = new cO(k.tooltip);
      de();
      if (l) {
        l.apply(bo, [bo])
      }
      bt(bo.callbacks, function (a) {
        a.apply(bo, [bo])
      });
      bA(bo, "load")
    }

    function df() {
      var a, b = bc && bc.parentNode;
      if (bo === null) {
        return
      }
      bA(bo, "destroy");
      bz(bo);
      a = cw.length;
      while (a--) {
        cw[a] = cw[a].destroy()
      }
      a = cy.length;
      while (a--) {
        cy[a] = cy[a].destroy()
      }
      bt(["title", "subtitle", "seriesGroup", "clipRect", "credits", "tracker", "scroller", "rangeSelector"], function (a) {
        var b = bo[a];
        if (b) {
          bo[a] = b.destroy()
        }
      });
      bt([bk, bn, bl, ci, bw, cA, cf], function (a) {
        if (a && a.destroy) {
          a.destroy()
        }
      });
      bk = bn = bl = ci = bw = cA = cf = null;
      if (bc) {
        bc.innerHTML = "";
        bz(bc);
        if (b) {
          cj(bc)
        }
        bc = null
      }
      clearInterval(cC);
      for (a in bo) {
        delete bo[a]
      }
      bo = null;
      k = null
    }

    function de() {
      var a = k.labels,
        b = k.credits,
        c;
      c$();
      ci = bo.legend = new cP;
      bt(cw, function (a) {
        a.setScale()
      });
      cF();
      bt(cw, function (a) {
        a.setTickPositions(true)
      });
      cS();
      cF();
      cE();
      if (ct) {
        bt(cw, function (a) {
          a.render()
        })
      }
      if (!bo.seriesGroup) {
        bo.seriesGroup = cA.g("series-group").attr({
          zIndex: 3
        }).add()
      }
      bt(cy, function (a) {
        a.translate();
        a.setTooltipPoints();
        a.render()
      });
      if (a.items) {
        bt(a.items, function () {
          var b = bE(a.style, this.style),
            c = bG(b.left) + $,
            d = bG(b.top) + X + 12;
          delete b.left;
          delete b.top;
          cA.text(this.html, c, d).attr({
            zIndex: 2
          }).css(b).add()
        })
      }
      if (!bo.toolbar) {
        bo.toolbar = cM()
      }
      if (b.enabled && !bo.credits) {
        c = b.href;
        bo.credits = cA.text(b.text, 0, 0).on("click", function () {
          if (c) {
            location.href = c
          }
        }).attr({
          align: b.position.align,
          zIndex: 8
        }).css(b.style).add().align(b.position)
      }
      ch();
      bo.hasRendered = true;
      if (bb) {
        ba.appendChild(bc);
        cj(bb)
      }
    }

    function dd() {
      var a = "bar",
        b = cz || n.inverted || n.type === a || n.defaultSeriesType === a,
        c = k.series,
        d = c && c.length;
      while (!b && d--) {
        if (c[d].type === a) {
          b = true
        }
      }
      bo.inverted = cz = b
    }

    function dc() {
      if (bo) {
        bA(bo, "endResize", null, function () {
          cu -= 1
        })
      }
    }

    function db() {
      function b() {
        var b = n.width || ba.offsetWidth,
          c = n.height || ba.offsetHeight;
        if (b && c) {
          if (b !== be || c !== bf) {
            clearTimeout(a);
            a = setTimeout(function () {
              cI(b, c, false)
            }, 100)
          }
          be = b;
          bf = c
        }
      }
      var a;
      by(c, "resize", b);
      by(bo, "destroy", function () {
        bz(c, "resize", b)
      })
    }

    function da() {
      ba = n.renderTo;
      bd = K + z++;
      if (bH(ba)) {
        ba = b.getElementById(ba)
      }
      ba.innerHTML = "";
      if (!ba.offsetWidth) {
        bb = ba.cloneNode(0);
        bS(bb, {
          position: H,
          top: "-9999px",
          display: ""
        });
        b.body.appendChild(bb)
      }
      c_();
      bo.container = bc = bT(G, {
        className: K + "container" + (n.className ? " " + n.className : ""),
        id: bd
      }, bE({
        position: I,
        overflow: J,
        width: bg + M,
        height: bh + M,
        textAlign: "left",
        lineHeight: "normal"
      }, n.style), bb || ba);
      bo.renderer = cA = n.forExport ? new cv(bc, bg, bh, true) : new w(bc, bg, bh);
      var a, d;
      if (s && bc.getBoundingClientRect) {
        a = function () {
          bS(bc, {
            left: 0,
            top: 0
          });
          d = bc.getBoundingClientRect();
          bS(bc, {
            left: -(d.left - bG(d.left)) + M,
            top: -(d.top - bG(d.top)) + M
          })
        };
        a();
        by(c, "resize", a);
        by(bo, "destroy", function () {
          bz(c, "resize", a)
        })
      }
    }

    function c_() {
      be = (bb || ba).offsetWidth;
      bf = (bb || ba).offsetHeight;
      bo.chartWidth = bg = n.width || be || 600;
      bo.chartHeight = bh = n.height || (bf > 19 ? bf : 400)
    }

    function c$(a, b) {
      V = bx(k.title, a);
      W = bx(k.subtitle, b);
      bt([
        ["title", a, V],
        ["subtitle", b, W]
      ], function (a) {
        var b = a[0],
          c = bo[b],
          d = a[1],
          e = a[2];
        if (c && d) {
          c = c.destroy()
        }
        if (e && e.text && !c) {
          bo[b] = cA.text(e.text, 0, 0, e.useHTML).attr({
            align: e.align,
            "class": K + b,
            zIndex: 1
          }).css(e.style).add().align(e, false, U)
        }
      })
    }

    function cZ() {
      return bu(cy, function (a) {
        return a.selected
      })
    }

    function cY() {
      var a = [];
      bt(cy, function (b) {
        a = a.concat(bu(b.points, function (a) {
          return a.selected
        }))
      });
      return a
    }

    function cX() {
      var a = k.xAxis || {},
        b = k.yAxis || {},
        c, d;
      a = bQ(a);
      bt(a, function (a, b) {
        a.index = b;
        a.isX = true
      });
      b = bQ(b);
      bt(b, function (a, b) {
        a.index = b
      });
      c = a.concat(b);
      bt(c, function (a) {
        d = new cL(a)
      });
      cS()
    }

    function cW(a) {
      var b, c, d;
      for (b = 0; b < cw.length; b++) {
        if (cw[b].options.id === a) {
          return cw[b]
        }
      }
      for (b = 0; b < cy.length; b++) {
        if (cy[b].options.id === a) {
          return cy[b]
        }
      }
      for (b = 0; b < cy.length; b++) {
        d = cy[b].points;
        for (c = 0; c < d.length; c++) {
          if (d[c].id === a) {
            return d[c]
          }
        }
      }
      return null
    }

    function cV() {
      if (bJ) {
        bB(bJ, {
          opacity: 0
        }, {
            duration: k.loading.hideDuration || 100,
            complete: function () {
              bS(bJ, {
                display: N
              })
            }
          })
      }
      bU = false
    }

    function cU(a) {
      var b = k.loading;
      if (!bJ) {
        bJ = bT(G, {
          className: K + "loading"
        }, bE(b.style, {
          left: $ + M,
          top: X + M,
          width: ce + M,
          height: cd + M,
          zIndex: 10,
          display: N
        }), bc);
        bK = bT("span", null, b.labelStyle, bJ)
      }
      bK.innerHTML = a || k.lang.loading;
      if (!bU) {
        bS(bJ, {
          opacity: 0,
          display: ""
        });
        bB(bJ, {
          opacity: b.style.opacity
        }, {
            duration: b.showDuration || 0
          });
        bU = true
      }
    }

    function cT(a) {
      var b = bo.isDirtyLegend,
        c, d = bo.isDirtyBox,
        e = cy.length,
        f = e,
        g = bo.clipRect,
        h;
      cc(a, bo);
      while (f--) {
        h = cy[f];
        if (h.isDirty && h.options.stacking) {
          c = true;
          break
        }
      }
      if (c) {
        f = e;
        while (f--) {
          h = cy[f];
          if (h.options.stacking) {
            h.isDirty = true
          }
        }
      }
      bt(cy, function (a) {
        if (a.isDirty) {
          if (a.options.legendType === "point") {
            b = true
          }
        }
      });
      if (b && ci.renderLegend) {
        ci.renderLegend();
        bo.isDirtyLegend = false
      }
      if (ct) {
        if (!cu) {
          cx = null;
          bt(cw, function (a) {
            a.setScale()
          })
        }
        cS();
        cF();
        bt(cw, function (a) {
          if (a.isDirty) {
            a.redraw()
          }
        })
      }
      if (d) {
        cE();
        ch();
        if (g) {
          bC(g);
          g.animate({
            width: bo.plotSizeX,
            height: bo.plotSizeY + 1
          })
        }
      }
      bt(cy, function (a) {
        if (a.isDirty && a.visible && (!a.isCartesian || a.xAxis)) {
          a.redraw()
        }
      });
      if (cf && cf.resetTracker) {
        cf.resetTracker()
      }
      bA(bo, "redraw")
    }

    function cS() {
      if (n.alignTicks !== false) {
        bt(cw, function (a) {
          a.adjustTickAmount()
        })
      }
      cx = null
    }

    function cR(a, b, c) {
      var d;
      if (a) {
        cc(c, bo);
        b = bR(b, true);
        bA(bo, "addSeries", {
          options: a
        }, function () {
          d = cQ(a);
          d.isDirty = true;
          bo.isDirtyLegend = true;
          if (b) {
            bo.redraw()
          }
        })
      }
      return d
    }

    function cQ(a) {
      var b = a.type || n.type || n.defaultSeriesType,
        c = bD[b],
        d, e = bo.hasRendered;
      if (e) {
        if (cz && b === "column") {
          c = bD.bar
        } else if (!cz && b === "bar") {
          c = bD.column
        }
      }
      d = new c;
      d.init(bo, a);
      if (!e && d.inverted) {
        cz = true
      }
      if (d.isCartesian) {
        ct = d.isCartesian
      }
      cy.push(d);
      return d
    }

    function cO(a) {
      function C() {
        if (bo.trackerGroup) {
          bo.trackerGroup = cg = bo.trackerGroup.destroy()
        }
        bz(bc, "mouseleave", A);
        bz(b, "mousemove", z);
        bc.onclick = bc.onmousedown = bc.onmousemove = bc.ontouchstart = bc.ontouchend = bc.ontouchmove = null
      }

      function B() {
        var a = true;
        bc.onmousedown = function (a) {
          a = t(a);
          if (!x && a.preventDefault) {
            a.preventDefault()
          }
          bo.mouseIsDown = bF = true;
          bo.mouseDownX = d = a.chartX;
          f = a.chartY;
          by(b, x ? "touchend" : "mouseup", y)
        };
        var c = function (b) {
          if (b && b.touches && b.touches.length > 1) {
            return
          }
          b = t(b);
          if (!x) {
            b.returnValue = false
          }
          var c = b.chartX,
            e = b.chartY,
            h = !bs(c - $, e - X);
          if (x && b.type === "touchstart") {
            if (bP(b.target, "isTracker")) {
              if (!bo.runTrackerClick) {
                b.preventDefault()
              }
            } else if (!bq && !h) {
              b.preventDefault()
            }
          }
          if (h) {
            if (c < $) {
              c = $
            } else if (c > $ + ce) {
              c = $ + ce
            }
            if (e < X) {
              e = X
            } else if (e > X + cd) {
              e = X + cd
            }
          }
          if (bF && b.type !== "touchstart") {
            g = Math.sqrt(Math.pow(d - c, 2) + Math.pow(f - e, 2));
            if (g > 10) {
              var i = bs(d - $, f - X);
              if (ct && (m || o) && i) {
                if (!k) {
                  k = cA.rect($, X, q ? 1 : ce, s ? 1 : cd, 0).attr({
                    fill: n.selectionMarkerFill || "rgba(69,114,167,0.25)",
                    zIndex: 7
                  }).add()
                }
              }
              if (k && q) {
                var l = c - d;
                k.attr({
                  width: j(l),
                  x: (l > 0 ? 0 : l) + d
                })
              }
              if (k && s) {
                var p = e - f;
                k.attr({
                  height: j(p),
                  y: (p > 0 ? 0 : p) + f
                })
              }
              if (i && !k && n.panning) {
                bo.pan(c)
              }
            }
          } else if (!h) {
            v(b)
          }
          a = h;
          return h || !ct
        };
        bc.onmousemove = c;
        by(bc, "mouseleave", A);
        by(b, "mousemove", z);
        bc.ontouchstart = function (a) {
          if (m || o) {
            bc.onmousedown(a)
          }
          c(a)
        };
        bc.ontouchmove = c;
        bc.ontouchend = function () {
          if (g) {
            w()
          }
        };
        bc.onclick = function (a) {
          var b = bo.hoverPoint;
          a = t(a);
          a.cancelBubble = true;
          if (!g) {
            if (b && bP(a.target, "isTracker")) {
              var c = b.plotX,
                d = b.plotY;
              bE(b, {
                pageX: cs.left + $ + (cz ? ce - d : c),
                pageY: cs.top + X + (cz ? cd - c : d)
              });
              bA(b.series, "click", bE(a, {
                point: b
              }));
              b.firePointEvent("click", a)
            } else {
              bE(a, u(a));
              if (bs(a.chartX - $, a.chartY - X)) {
                bA(bo, "click", a)
              }
            }
          }
          g = false
        }
      }

      function A() {
        w();
        cs = null
      }

      function z(a) {
        var b = bO(a.pageX) ? a.pageX : a.page.x,
          c = bO(a.pageX) ? a.pageY : a.page.y;
        if (cs && !bs(b - cs.left - $, c - cs.top - X)) {
          w()
        }
      }

      function y() {
        if (k) {
          var a = {
            xAxis: [],
            yAxis: []
          },
            c = k.getBBox(),
            d = c.x - $,
            e = c.y - X;
          if (g) {
            bt(cw, function (b) {
              if (b.options.zoomEnabled !== false) {
                var f = b.translate,
                  g = b.isXAxis,
                  j = cz ? !g : g,
                  k = f(j ? d : cd - e - c.height, true, 0, 0, 1),
                  l = f(j ? d + c.width : cd - e, true, 0, 0, 1);
                a[g ? "xAxis" : "yAxis"].push({
                  axis: b,
                  min: i(k, l),
                  max: h(k, l)
                })
              }
            });
            bA(bo, "selection", a, cJ)
          }
          k = k.destroy()
        }
        bS(bc, {
          cursor: "auto"
        });
        bo.mouseIsDown = bF = g = false;
        bz(b, x ? "touchend" : "mouseup", y)
      }

      function w() {
        var a = bo.hoverSeries,
          b = bo.hoverPoint;
        if (b) {
          b.onMouseOut()
        }
        if (a) {
          a.onMouseOut()
        }
        if (bw) {
          bw.hide();
          bw.hideCrosshairs()
        }
        cD = null
      }

      function v(b) {
        var c, d, e = bo.hoverPoint,
          f = bo.hoverSeries,
          g, h, k = bg,
          l = cz ? b.chartY : b.chartX - $;
        if (bw && a.shared && !(f && f.noSharedTooltip)) {
          d = [];
          g = cy.length;
          for (h = 0; h < g; h++) {
            if (cy[h].visible && cy[h].options.enableMouseTracking !== false && !cy[h].noSharedTooltip && cy[h].tooltipPoints.length) {
              c = cy[h].tooltipPoints[parseInt(l)];
              c._dist = j(l - c.plotX);
              k = i(k, c._dist);
              d.push(c)
            }
          }
          g = d.length;
          while (g--) {
            if (d[g]._dist > k) {
              d.splice(g, 1)
            }
          }
          if (d.length && d[0].plotX !== cD) {
            bw.refresh(d);
            cD = d[0].plotX
          }
        }
        if (f && f.tracker) {
          c = f.tooltipPoints[l];
          if (c && c !== e) {
            c.onMouseOver()
          }
        }
      }

      function u(a) {
        var b = {
          xAxis: [],
          yAxis: []
        };
        bt(cw, function (c) {
          var d = c.translate,
            e = c.isXAxis,
            f = cz ? !e : e;
          b[e ? "xAxis" : "yAxis"].push({
            axis: c,
            value: d(f ? a.chartX - $ : cd - a.chartY + X, true)
          })
        });
        return b
      }

      function t(a) {
        var d, f = r && b.width / b.body.scrollWidth - 1,
          g, h, i, j;
        a = a || c.event;
        if (!a.target) {
          a.target = a.srcElement
        }
        if (a.originalEvent) {
          a = a.originalEvent
        }
        if (a.event) {
          a = a.event
        }
        d = a.touches ? a.touches.item(0) : a;
        cs = bv(bc);
        g = cs.left;
        h = cs.top;
        if (p) {
          i = a.x;
          j = a.y
        } else {
          i = d.pageX - g;
          j = d.pageY - h
        }
        if (f) {
          i += e((f + 1) * g - g);
          j += e((f + 1) * h - h)
        }
        return bE(a, {
          chartX: i,
          chartY: j
        })
      }
      var d, f, g, k, l = n.zoomType,
        m = /x/.test(l),
        o = /y/.test(l),
        q = m && !cz || o && cz,
        s = o && !cz || m && cz;
      ch = function () {
        if (!cg) {
          bo.trackerGroup = cg = cA.g("tracker").attr({
            zIndex: 9
          }).add()
        } else {
          cg.translate($, X);
          if (cz) {
            cg.attr({
              width: bo.plotWidth,
              height: bo.plotHeight
            }).invert()
          }
        }
      };
      ch();
      if (a.enabled) {
        bo.tooltip = bw = cN(a)
      }
      B();
      cC = setInterval(function () {
        if (cB) {
          cB()
        }
      }, 32);
      bE(this, {
        zoomX: m,
        zoomY: o,
        resetTracker: w,
        normalizeMouseEvent: t,
        destroy: C
      })
    }

    function cN(a) {
      function t(c) {
        var g, i, j, l, m, o = {},
          s, t = [],
          u = c.tooltipPos,
          v = a.formatter || p,
          w = bo.hoverPoints,
          x;
        if (h && !(c.series && c.series.noSharedTooltip)) {
          m = 0;
          if (w) {
            bt(w, function (a) {
              a.setState()
            })
          }
          bo.hoverPoints = c;
          bt(c, function (a) {
            a.setState(S);
            m += a.plotY;
            t.push(a.getLabelConfig())
          });
          l = c[0].plotX;
          m = e(m) / c.length;
          o = {
            x: c[0].category
          };
          o.points = t;
          c = c[0]
        } else {
          o = c.getLabelConfig()
        }
        s = v.call(o);
        b = c.series;
        l = bR(l, c.plotX);
        m = bR(m, c.plotY);
        g = e(u ? u[0] : cz ? ce - m : l);
        i = e(u ? u[1] : cz ? cd - l : m);
        j = h || !c.series.isCartesian || bs(g, i);
        if (s === false || !j) {
          r()
        } else {
          if (k) {
            n.show();
            k = false
          }
          n.attr({
            text: s
          });
          n.attr({
            stroke: a.borderColor || c.color || b.color || "#606060"
          });
          x = bZ(n.width, n.height, $, X, ce, cd, {
            x: g,
            y: i
          }, bR(a.distance, 12));
          q(e(x.x), e(x.y))
        }
        if (d) {
          d = bQ(d);
          var y, z = d.length,
            A, B;
          while (z--) {
            B = c.series[z ? "yAxis" : "xAxis"];
            if (d[z] && B) {
              y = B.getPlotLinePath(c[z ? "y" : "x"], 1);
              if (f[z]) {
                f[z].attr({
                  d: y,
                  visibility: L
                })
              } else {
                A = {
                  "stroke-width": d[z].width || 1,
                  stroke: d[z].color || "#C0C0C0",
                  zIndex: d[z].zIndex || 2
                };
                if (d[z].dashStyle) {
                  A.dashstyle = d[z].dashStyle
                }
                f[z] = cA.path(y).attr(A).add()
              }
            }
          }
        }
      }

      function s() {
        bt(f, function (a) {
          if (a) {
            a.hide()
          }
        })
      }

      function r() {
        if (!k) {
          var a = bo.hoverPoints;
          n.hide();
          if (a) {
            bt(a, function (a) {
              a.setState()
            })
          }
          bo.hoverPoints = null;
          k = true
        }
      }

      function q(a, b) {
        l = k ? a : (2 * l + a) / 3;
        m = k ? b : (m + b) / 2;
        n.attr({
          x: l,
          y: m
        });
        if (j(a - l) > 1 || j(b - m) > 1) {
          cB = function () {
            q(a, b)
          }
        } else {
          cB = null
        }
      }

      function p() {
        var a = this,
          b = a.points || bQ(a),
          c = b[0].series,
          d;
        d = [c.tooltipHeaderFormatter(b[0].key)];
        bt(b, function (a) {
          c = a.series;
          d.push(c.tooltipFormatter && c.tooltipFormatter(a) || a.point.tooltipFormatter(c.tooltipOptions.pointFormat))
        });
        return d.join("")
      }

      function o() {
        bt(f, function (a) {
          if (a) {
            a.destroy()
          }
        });
        if (n) {
          n = n.destroy()
        }
      }
      var b, c = a.borderWidth,
        d = a.crosshairs,
        f = [],
        g = a.style,
        h = a.shared,
        i = bG(g.padding),
        k = true,
        l = 0,
        m = 0;
      g.padding = 0;
      var n = cA.label("", 0, 0).attr({
        padding: i,
        fill: a.backgroundColor,
        "stroke-width": c,
        r: a.borderRadius,
        zIndex: 8
      }).css(g).hide().add().shadow(a.shadow);
      return {
        shared: h,
        refresh: t,
        hide: r,
        hideCrosshairs: s,
        destroy: o
      }
    }

    function cM() {
      function c(b) {
        cj(a[b].element);
        a[b] = null
      }

      function b(b, c, d, e) {
        if (!a[b]) {
          var f = cA.text(c, 0, 0).css(k.toolbar.itemStyle).align({
            align: "right",
            x: -Y - 20,
            y: X + 30
          }).on("click", e).attr({
            align: "right",
            zIndex: 20
          }).add();
          a[b] = f
        }
      }
      var a = {};
      return {
        add: b,
        remove: c
      }
    }

    function cL(b) {
      function cI() {
        var a;
        bz(q);
        for (a in o) {
          cb(o[a]);
          o[a] = null
        }
        if (q.stackTotalGroup) {
          q.stackTotalGroup = q.stackTotalGroup.destroy()
        }
        bt([bH, bI, bJ, bv], function (a) {
          cb(a)
        });
        bt([S, Q, R, r], function (a) {
          if (a) {
            a.destroy()
          }
        });
        S = Q = R = r = null
      }

      function cH(a, c) {
        q.categories = b.categories = bS = a;
        bt(q.series, function (a) {
          a.translate();
          a.setTooltipPoints(true)
        });
        q.isDirty = true;
        if (bR(c, true)) {
          bo.redraw()
        }
      }

      function cG() {
        if (cf.resetTracker) {
          cf.resetTracker()
        }
        if (p.ordinal) {
          cr(true)
        }
        cE();
        bt(bv, function (a) {
          a.render()
        });
        bt(q.series, function (a) {
          a.isDirty = true
        })
      }

      function cF(a) {
        var b = bv.length;
        while (b--) {
          if (bv[b].id === a) {
            bv[b].destroy()
          }
        }
      }

      function cE() {
        var b = p.title,
          c = p.stackLabels,
          d = p.alternateGridColor,
          e = p.lineWidth,
          f, g, h, i = bo.hasRendered,
          j = i && bO(be) && !isNaN(be),
          n = q.series.length && bO(bd) && bO(bc),
          s = n || bR(p.showEmpty, true);
        if (n || bn) {
          if (bB && !bS) {
            var t = bd + (bD[0] - bd) % bB;
            for (; t <= bc; t += bB) {
              if (!bI[t]) {
                bI[t] = new b$(t, "minor")
              }
              if (j && bI[t].isNew) {
                bI[t].render(null, true)
              }
              bI[t].isActive = true;
              bI[t].render()
            }
          }
          bt(bD, function (a, b) {
            if (!bn || a >= bd && a <= bc) {
              if (!bH[a]) {
                bH[a] = new b$(a)
              }
              if (j && bH[a].isNew) {
                bH[a].render(b, true)
              }
              bH[a].isActive = true;
              bH[a].render(b)
            }
          });
          if (d) {
            bt(bD, function (b, c) {
              if (c % 2 === 0 && b < bc) {
                if (!bJ[b]) {
                  bJ[b] = new cc
                }
                bJ[b].options = {
                  from: b,
                  to: bD[c + 1] !== a ? bD[c + 1] : bc,
                  color: d
                };
                bJ[b].render();
                bJ[b].isActive = true
              }
            })
          }
          if (!q._addedPlotLB) {
            bt((p.plotLines || []).concat(p.plotBands || []), function (a) {
              cC(a)
            });
            q._addedPlotLB = true
          }
        }
        bt([bH, bI, bJ], function (a) {
          var b;
          for (b in a) {
            if (!a[b].isActive) {
              a[b].destroy();
              delete a[b]
            } else {
              a[b].isActive = false
            }
          }
        });
        if (e) {
          f = D + (k ? F : 0) + v;
          g = bh - H - (k ? G : 0) + v;
          h = cA.crispLine([O, l ? D : f, l ? g : E, P, l ? bg - I : f, l ? g : bh - H], e);
          if (!S) {
            S = cA.path(h).attr({
              stroke: p.lineColor,
              "stroke-width": e,
              zIndex: 7
            }).add()
          } else {
            S.animate({
              d: h
            })
          }
          S[s ? "show" : "hide"]()
        }
        if (r && s) {
          var u = l ? D : E,
            w = bG(b.style.fontSize || 12),
            y = {
              low: u + (l ? 0 : x),
              middle: u + x / 2,
              high: u + (l ? x : 0)
            }[b.align],
            z = (l ? E + G : D) + (l ? 1 : -1) * (k ? -1 : 1) * bQ + (m === 2 ? w : 0);
          r[r.isNew ? "attr" : "animate"]({
            x: l ? y : z + (k ? F : 0) + v + (b.x || 0),
            y: l ? z - (k ? G : 0) + v : y + (b.y || 0)
          });
          r.isNew = false
        }
        if (c && c.enabled) {
          var A, B, C, J = q.stackTotalGroup;
          if (!J) {
            q.stackTotalGroup = J = cA.g("stack-labels").attr({
              visibility: L,
              zIndex: 6
            }).translate($, X).add()
          }
          for (A in o) {
            B = o[A];
            for (C in B) {
              B[C].render(J)
            }
          }
        }
        q.isDirty = false
      }

      function cD() {
        var a = q.series.length && bO(bd) && bO(bc),
          b = a || bR(p.showEmpty, true),
          c = 0,
          d = 0,
          e = p.title,
          f = p.labels,
          g = [-1, 1, 1, -1][m],
          i;
        if (!Q) {
          Q = cA.g("axis").attr({
            zIndex: 7
          }).add();
          R = cA.g("grid").attr({
            zIndex: p.gridZIndex || 1
          }).add()
        }
        bP = 0;
        if (a || bn) {
          bt(bD, function (a) {
            if (!bH[a]) {
              bH[a] = new b$(a)
            } else {
              bH[a].addLabel()
            }
          });
          bt(bD, function (a) {
            if (m === 0 || m === 2 || {
              1: "left",
              3: "right"
            }[m] === f.align) {
              bP = h(bH[a].getLabelSize(), bP)
            }
          });
          if (bU) {
            bP += (bU - 1) * 16
          }
        } else {
          for (i in bH) {
            bH[i].destroy();
            delete bH[i]
          }
        }
        if (e && e.text) {
          if (!r) {
            r = q.axisTitle = cA.text(e.text, 0, 0, e.useHTML).attr({
              zIndex: 7,
              rotation: e.rotation || 0,
              align: e.textAlign || {
                low: "left",
                middle: "center",
                high: "right"
              }[e.align]
            }).css(e.style).add();
            r.isNew = true
          }
          if (b) {
            c = r.getBBox()[l ? "height" : "width"];
            d = bR(e.margin, l ? 5 : 10)
          }
          r[b ? "show" : "hide"]()
        }
        v = g * bR(p.offset, _[m]);
        bQ = bR(e.offset, bP + d + (m !== 2 && bP && g * p.labels[l ? "y" : "x"]));
        _[m] = h(_[m], bQ + c + g * v)
      }

      function cC(a) {
        var b = (new cc(a)).render();
        bv.push(b);
        return b
      }

      function cB(a) {
        if (bd > a || a === null) {
          a = bd
        } else if (bc < a) {
          a = bc
        }
        return K(a, 0, 1)
      }

      function cy() {
        return {
          min: bd,
          max: bc,
          dataMin: T,
          dataMax: U,
          userMin: Y,
          userMax: Z
        }
      }

      function cv() {
        var a = p.offsetLeft || 0,
          b = p.offsetRight || 0,
          d = bc - bd,
          e = 0,
          f, g;
        D = bR(p.left, $ + a);
        E = bR(p.top, X);
        F = bR(p.width, ce - a + b);
        G = bR(p.height, cd);
        H = bh - G - E;
        I = bg - F - D;
        x = l ? F : G;
        if (c) {
          bt(q.series, function (a) {
            e = h(e, a.pointRange);
            g = a.closestPointRange;
            if (!a.noSharedTooltip && bO(g)) {
              f = bO(f) ? i(f, g) : g
            }
          });
          if ((bO(Y) || bO(Z)) && e > bw / 2) {
            e = 0
          }
          q.pointRange = e;
          q.closestPointRange = f
        }
        q.translationSlope = z = x / (d + e || 1);
        A = l ? D : H;
        bm = z * (e / 2);
        q.left = D;
        q.top = E;
        q.len = x
      }

      function cu(a, b, c, d) {
        c = bR(c, true);
        bA(q, "setExtremes", {
          min: a,
          max: b
        }, function () {
          Y = a;
          Z = b;
          if (c) {
            bo.redraw(d)
          }
        });
        bA(q, "afterSetExtremes", {
          min: bd,
          max: bc
        })
      }

      function ct() {
        var a, b, d;
        be = bd;
        bf = bc;
        y = x;
        x = l ? F : G;
        bt(q.series, function (a) {
          if (a.isDirtyData || a.isDirty || a.xAxis.isDirty) {
            d = true
          }
        });
        if (x !== y || d || bn || Y !== ba || Z !== bb) {
          ch();
          cr();
          ba = Y;
          bb = Z;
          B = z;
          q.translationSlope = z = x / (bc - bd + (q.pointRange || 0) || 1);
          if (!c) {
            for (a in o) {
              for (b in o[a]) {
                o[a][b].cum = o[a][b].total
              }
            }
          }
          if (!q.isDirty) {
            q.isDirty = bo.isDirtyBox || bd !== be || bc !== bf
          }
        }
      }

      function cs() {
        if (cx && cx[w] && !t && !bS && !bn && p.alignTicks !== false) {
          var a = bK,
            b = bD.length;
          bK = cx[w];
          if (b < bK) {
            while (bD.length < bK) {
              bD.push(ci(bD[bD.length - 1] + bw))
            }
            z *= (b - 1) / (bK - 1);
            bc = bD[bD.length - 1]
          }
          if (bO(a) && bK !== a) {
            q.isDirty = true
          }
        }
      }

      function cr(a) {
        var b, e, g, i = p.tickInterval,
          j = p.tickPixelInterval;
        if (a && q.beforeSetTickPositions) {
          q.beforeSetTickPositions()
        }
        if (bn) {
          e = bo[c ? "xAxis" : "yAxis"][p.linkedTo];
          g = e.getExtremes();
          bd = bR(g.min, g.dataMin);
          bc = bR(g.max, g.dataMax)
        } else {
          bd = bR(Y, p.min, T);
          bc = bR(Z, p.max, U)
        }
        if (u) {
          bd = bL(bd);
          bc = bL(bc)
        }
        if (W) {
          Y = bd = h(bd, bc - W);
          Z = bc;
          if (a) {
            W = null
          }
        }
        ck(a);
        if (!bS && !br && !bn && bO(bd) && bO(bc)) {
          b = bc - bd || 1;
          if (!bO(p.min) && !bO(Y) && bk && (T < 0 || !bp)) {
            bd -= b * bk
          }
          if (!bO(p.max) && !bO(Z) && bl && (U > 0 || !bq)) {
            bc += b * bl
          }
        }
        if (bd === bc || bd === undefined || bc === undefined) {
          bw = 1
        } else if (bn && !i && j === e.options.tickPixelInterval) {
          bw = e.tickInterval
        } else {
          bw = bR(i, bS ? 1 : (bc - bd) * j / (x || 1))
        }
        if (a && q.postProcessTickInterval) {
          bw = q.postProcessTickInterval(bw)
        }
        if (!t) {
          bC = d.pow(10, f(d.log(bw) / d.LN10));
          if (!bO(p.tickInterval)) {
            bw = bW(bw, null, bC, p)
          }
        }
        q.tickInterval = bw;
        bB = p.minorTickInterval === "auto" && bw ? bw / 5 : p.minorTickInterval;
        bD = p.tickPositions || bF && bF.apply(q, [bd, bc]);
        if (!bD) {
          if (t) {
            bD = bX(bw, bd, bc, p.startOfWeek, p.units)
          } else {
            cj()
          }
        }
        if (a) {
          bA(q, "afterSetTickPositions", {
            tickPositions: bD
          })
        }
        if (!bn) {
          var k = bD[0],
            l = bD[bD.length - 1];
          if (p.startOnTick) {
            bd = k
          } else if (bd > k) {
            bD.shift()
          }
          if (p.endOnTick) {
            bc = l
          } else if (bc < l) {
            bD.pop()
          }
          if (!cx) {
            cx = {
              x: 0,
              y: 0
            }
          }
          if (!t && bD.length > cx[w] && p.alignTicks !== false) {
            cx[w] = bD.length
          }
        }
      }

      function ck(b) {
        var d, e = (q.pointRange || 0) / 2,
          f = U - T > V,
          g, h;
        if (b && V === a) {
          V = c && !bO(p.min) && !bO(p.max) ? i(q.closestPointRange * 5, U - T) : null
        }
        if (bc - bd < V) {
          d = (V - bc + bd) / 2;
          g = [bd - d, bR(p.min, bd - d)];
          if (f) {
            g[2] = T - e
          }
          bd = ca(g);
          h = [bd + V, bR(p.max, bd + V)];
          if (f) {
            h[2] = U + e
          }
          bc = b_(h);
          if (bc - bd < V) {
            g[0] = bc - V;
            g[1] = bR(p.min, bc - V);
            bd = ca(g)
          }
        }
      }

      function cj() {
        var a, b, c = ci(f(bd / bw) * bw),
          d = ci(g(bc / bw) * bw);
        bD = [];
        a = c;
        while (a <= d) {
          bD.push(a);
          a = ci(a + bw);
          if (a === b) {
            break
          }
          b = a
        }
      }

      function ci(a) {
        var b, c = a;
        bC = bR(bC, d.pow(10, f(d.log(bw) / d.LN10)));
        if (bC < 1) {
          b = e(1 / bC) * 10;
          c = e(a * b) / b
        }
        return c
      }

      function ch() {
        var b = [],
          d = [],
          e;
        T = U = null;
        bt(q.series, function (f) {
          if (f.visible || !n.ignoreHiddenSeries) {
            var g = f.options,
              j, k, l, m, q, r, s, t, u, v, w = g.threshold,
              x, y = [],
              z = 0;
            if (c) {
              s = f.xData;
              if (s.length) {
                T = i(bR(T, s[0]), b_(s));
                U = h(bR(U, s[0]), ca(s))
              }
            } else {
              var A, B, C, D = f.cropped,
                E = f.xAxis.getExtremes(),
                F, G = !!f.modifyValue;
              j = g.stacking;
              br = j === "percent";
              if (j) {
                q = g.stack;
                m = f.type + bR(q, "");
                r = "-" + m;
                f.stackKey = m;
                k = b[m] || [];
                b[m] = k;
                l = d[r] || [];
                d[r] = l
              }
              if (br) {
                T = 0;
                U = 99
              }
              f.processData();
              s = f.processedXData;
              t = f.processedYData;
              x = t.length;
              for (e = 0; e < x; e++) {
                u = s[e];
                v = t[e];
                if (v !== null && v !== a) {
                  if (j) {
                    A = v < w;
                    B = A ? l : k;
                    C = A ? r : m;
                    v = B[u] = bO(B[u]) ? B[u] + v : v;
                    if (!o[C]) {
                      o[C] = {}
                    }
                    if (!o[C][u]) {
                      o[C][u] = new cg(p.stackLabels, A, u, q)
                    }
                    o[C][u].setTotal(v)
                  } else if (G) {
                    v = f.modifyValue(v)
                  }
                  if (D || (s[e + 1] || u) >= E.min && (s[e - 1] || u) <= E.max) {
                    F = v.length;
                    if (F) {
                      while (F--) {
                        if (v[F] !== null) {
                          y[z++] = v[F]
                        }
                      }
                    } else {
                      y[z++] = v
                    }
                  }
                }
              }
              if (!br && y.length) {
                T = i(bR(T, y[0]), b_(y));
                U = h(bR(U, y[0]), ca(y))
              }
              if (f.useThreshold && w !== null) {
                if (T >= w) {
                  T = w;
                  bp = true
                } else if (U < w) {
                  U = w;
                  bq = true
                }
              }
            }
          }
        })
      }

      function cg(a, b, c, d) {
        var e = this;
        e.isNegative = b;
        e.options = a;
        e.x = c;
        e.stack = d;
        e.alignOptions = {
          align: a.align || (cz ? b ? "left" : "right" : "center"),
          verticalAlign: a.verticalAlign || (cz ? "middle" : b ? "bottom" : "top"),
          y: bR(a.y, cz ? 4 : b ? 14 : -6),
          x: bR(a.x, cz ? b ? -6 : 6 : 0)
        };
        e.textAlign = a.textAlign || (cz ? b ? "right" : "left" : "center")
      }

      function cc(a) {
        var b = this;
        if (a) {
          b.options = a;
          b.id = a.id
        }
        return b
      }

      function b$(a, b) {
        var c = this;
        c.pos = a;
        c.type = b || "";
        c.isNew = true;
        if (!b) {
          c.addLabel()
        }
      }
      var c = b.isX,
        k = b.opposite,
        l = cz ? !c : c,
        m = l ? k ? 0 : 2 : k ? 1 : 3,
        o = {},
        p = bx(c ? cl : cm, [cq, co, cp, cn][m], b),
        q = this,
        r, s = p.type,
        t = s === "datetime",
        u = s === "logarithmic",
        v = p.offset || 0,
        w = c ? "x" : "y",
        x = 0,
        y, z, A, B, D, E, F, G, H, I, K, N, Q, R, S, T, U, V = p.minRange || p.maxZoom,
        W = p.range,
        Y, Z, ba, bb, bc = null,
        bd = null,
        be, bf, bk = p.minPadding,
        bl = p.maxPadding,
        bm = 0,
        bn = bO(p.linkedTo),
        bp, bq, br, bs = p.events,
        bu, bv = [],
        bw, bB, bC, bD, bF = p.tickPositioner,
        bH = {},
        bI = {},
        bJ = {},
        bK, bP, bQ, bS = p.categories,
        bT = p.labels.formatter || function () {
          var a = this.value,
            b = this.dateTimeLabelFormat,
            c;
          if (b) {
            c = C(b, a)
          } else if (bw % 1e6 === 0) {
            c = a / 1e6 + "M"
          } else if (bw % 1e3 === 0) {
            c = a / 1e3 + "k"
          } else if (!bS && a >= 1e3) {
            c = bV(a, 0)
          } else {
            c = a
          }
          return c
        },
        bU = l && p.labels.staggerLines,
        bY = p.reversed,
        bZ = bS && p.tickmarkPlacement === "between" ? .5 : 0;
      b$.prototype = {
        addLabel: function () {
          var a = this,
            b = a.pos,
            c = p.labels,
            d, f = bS && l && bS.length && !c.step && !c.staggerLines && !c.rotation && ce / bS.length || !l && ce / 2,
            g = b === bD[0],
            i = b === bD[bD.length - 1],
            j, k = bS && bO(bS[b]) ? bS[b] : b,
            m = a.label,
            n, o;
          if (t) {
            n = bD.info;
            o = p.dateTimeLabelFormats[n.higherRanks[b] || n.unitName]
          }
          a.isFirst = g;
          a.isLast = i;
          d = bT.call({
            isFirst: g,
            isLast: i,
            dateTimeLabelFormat: o,
            value: u ? bM(k) : k
          });
          j = f && {
            width: h(1, e(f - 2 * (c.padding || 10))) + M
          };
          j = bE(j, c.style);
          if (!bO(m)) {
            a.label = bO(d) && c.enabled ? cA.text(d, 0, 0, c.useHTML).attr({
              align: c.align,
              rotation: c.rotation
            }).css(j).add(Q) : null
          } else if (m) {
            m.attr({
              text: d
            }).css(j)
          }
        },
        getLabelSize: function () {
          var a = this.label;
          return a ? (this.labelBBox = a.getBBox())[l ? "height" : "width"] : 0
        },
        render: function (b, c) {
          var d = this,
            e = d.type,
            f = d.label,
            g = d.pos,
            h = p.labels,
            i = d.gridLine,
            j = e ? e + "Grid" : "grid",
            m = e ? e + "Tick" : "tick",
            n = p[j + "LineWidth"],
            o = p[j + "LineColor"],
            q = p[j + "LineDashStyle"],
            r = p[m + "Length"],
            s = p[m + "Width"] || 0,
            t = p[m + "Color"],
            u = p[m + "Position"],
            w, x = d.mark,
            y, B = h.step,
            C = c && bj || bh,
            E, F, J;
          F = l ? K(g + bZ, null, null, c) + A : D + v + (k ? (c && bi || bg) - I - D : 0);
          J = l ? C - H + v - (k ? G : 0) : C - K(g + bZ, null, null, c) - A;
          if (n) {
            w = N(g + bZ, n, c);
            if (i === a) {
              E = {
                stroke: o,
                "stroke-width": n
              };
              if (q) {
                E.dashstyle = q
              }
              if (!e) {
                E.zIndex = 1
              }
              d.gridLine = i = n ? cA.path(w).attr(E).add(R) : null
            }
            if (!c && i && w) {
              i.animate({
                d: w
              })
            }
          }
          if (s) {
            if (u === "inside") {
              r = -r
            }
            if (k) {
              r = -r
            }
            y = cA.crispLine([O, F, J, P, F + (l ? 0 : -r), J + (l ? r : 0)], s);
            if (x) {
              x.animate({
                d: y
              })
            } else {
              d.mark = cA.path(y).attr({
                stroke: t,
                "stroke-width": s
              }).add(Q)
            }
          }
          if (f && !isNaN(F)) {
            F = F + h.x - (bZ && l ? bZ * z * (bY ? -1 : 1) : 0);
            J = J + h.y - (bZ && !l ? bZ * z * (bY ? 1 : -1) : 0);
            if (!bO(h.y)) {
              J += bG(f.styles.lineHeight) * .9 - f.getBBox().height / 2
            }
            if (bU) {
              J += b / (B || 1) % bU * 16
            }
            if (d.isFirst && !bR(p.showFirstLabel, 1) || d.isLast && !bR(p.showLastLabel, 1)) {
              f.hide()
            } else {
              f.show()
            }
            if (B && b % B) {
              f.hide()
            }
            f[d.isNew ? "attr" : "animate"]({
              x: F,
              y: J
            })
          }
          d.isNew = false
        },
        destroy: function () {
          cb(this)
        }
      };
      cc.prototype = {
        render: function () {
          var a = this,
            b = a.options,
            c = b.label,
            d = a.label,
            e = b.width,
            f = b.to,
            g = b.from,
            j = b.value,
            k, m = b.dashStyle,
            n = a.svgElem,
            o = [],
            p, q, r, s, t, v, w = b.color,
            x = b.zIndex,
            y = b.events,
            z;
          if (u) {
            g = bL(g);
            f = bL(f);
            j = bL(j)
          }
          if (e) {
            o = N(j, e);
            z = {
              stroke: w,
              "stroke-width": e
            };
            if (m) {
              z.dashstyle = m
            }
          } else if (bO(g) && bO(f)) {
            g = h(g, bd);
            f = i(f, bc);
            k = N(f);
            o = N(g);
            if (o && k) {
              o.push(k[4], k[5], k[1], k[2])
            } else {
              o = null
            }
            z = {
              fill: w
            }
          } else {
            return
          }
          if (bO(x)) {
            z.zIndex = x
          }
          if (n) {
            if (o) {
              n.animate({
                d: o
              }, null, n.onGetPath)
            } else {
              n.hide();
              n.onGetPath = function () {
                n.show()
              }
            }
          } else if (o && o.length) {
            a.svgElem = n = cA.path(o).attr(z).add();
            if (y) {
              p = function (b) {
                n.on(b, function (c) {
                  y[b].apply(a, [c])
                })
              };
              for (q in y) {
                p(q)
              }
            }
          }
          if (c && bO(c.text) && o && o.length && F > 0 && G > 0) {
            c = bx({
              align: l && k && "center",
              x: l ? !k && 4 : 10,
              verticalAlign: !l && k && "middle",
              y: l ? k ? 16 : 10 : k ? 6 : -4,
              rotation: l && !k && 90
            }, c);
            if (!d) {
              a.label = d = cA.text(c.text, 0, 0).attr({
                align: c.textAlign || c.align,
                rotation: c.rotation,
                zIndex: x
              }).css(c.style).add()
            }
            r = [o[1], o[4], bR(o[6], o[1])];
            s = [o[2], o[5], bR(o[7], o[2])];
            t = b_(r);
            v = b_(s);
            d.align(c, false, {
              x: t,
              y: v,
              width: ca(r) - t,
              height: ca(s) - v
            });
            d.show()
          } else if (d) {
            d.hide()
          }
          return a
        },
        destroy: function () {
          var a = this;
          cb(a);
          bN(bv, a)
        }
      };
      cg.prototype = {
        destroy: function () {
          cb(this)
        },
        setTotal: function (a) {
          this.total = a;
          this.cum = a
        },
        render: function (a) {
          var b = this,
            c = b.options.formatter.call(b);
          if (b.label) {
            b.label.attr({
              text: c,
              visibility: J
            })
          } else {
            b.label = bo.renderer.text(c, 0, 0).css(b.options.style).attr({
              align: b.textAlign,
              rotation: b.options.rotation,
              visibility: J
            }).add(a)
          }
        },
        setOffset: function (a, b) {
          var c = this,
            d = c.isNegative,
            e = q.translate(c.total),
            f = q.translate(0),
            g = j(e - f),
            h = bo.xAxis[0].translate(c.x) + a,
            i = bo.plotHeight,
            k = {
              x: cz ? d ? e : e - g : h,
              y: cz ? i - h - b : d ? i - e - g : i - e,
              width: cz ? g : b,
              height: cz ? b : g
            };
          if (c.label) {
            c.label.align(c.alignOptions, null, k).attr({
              visibility: L
            })
          }
        }
      };
      K = function (a, b, c, d, e) {
        var f = 1,
          g = 0,
          h = d ? B : z,
          i = d ? be : bd,
          j, k = p.ordinal || u && e;
        if (!h) {
          h = z
        }
        if (c) {
          f *= -1;
          g = x
        }
        if (bY) {
          f *= -1;
          g -= f * x
        }
        if (b) {
          if (bY) {
            a = x - a
          }
          j = a / h + i;
          if (k) {
            j = q.lin2val(j)
          }
        } else {
          if (k) {
            a = q.val2lin(a)
          }
          j = f * (a - i) * h + g + f * bm
        }
        return j
      };
      N = function (a, b, c) {
        var d, f, g, h, i = K(a, null, null, c),
          j = c && bj || bh,
          k = c && bi || bg,
          m;
        d = g = e(i + A);
        f = h = e(j - i - A);
        if (isNaN(i)) {
          m = true
        } else if (l) {
          f = E;
          h = j - H;
          if (d < D || d > D + F) {
            m = true
          }
        } else {
          d = D;
          g = k - I;
          if (f < E || f > E + G) {
            m = true
          }
        }
        return m ? null : cA.crispLine([O, d, f, P, g, h], b || 0)
      };
      cw.push(q);
      bo[c ? "xAxis" : "yAxis"].push(q);
      if (cz && c && bY === a) {
        bY = true
      }
      bE(q, {
        addPlotBand: cC,
        addPlotLine: cC,
        adjustTickAmount: cs,
        categories: bS,
        getExtremes: cy,
        getPlotLinePath: N,
        getThreshold: cB,
        isXAxis: c,
        options: p,
        plotLinesAndBands: bv,
        getOffset: cD,
        render: cE,
        setAxisSize: cv,
        setCategories: cH,
        setExtremes: cu,
        setScale: ct,
        setTickPositions: cr,
        translate: K,
        redraw: cG,
        removePlotBand: cF,
        removePlotLine: cF,
        reversed: bY,
        series: [],
        stacks: o,
        destroy: cI
      });
      for (bu in bs) {
        by(q, bu, bs[bu])
      }
      if (u) {
        q.val2lin = bL;
        q.lin2val = bM
      }
    }
    var m = k.series;
    k.series = null;
    k = bx(B, k);
    k.series = m;
    var n = k.chart,
      o = n.margin,
      q = bI(o) ? o : [o, o, o, o],
      t = bR(n.marginTop, q[0]),
      v = bR(n.marginRight, q[1]),
      y = bR(n.marginBottom, q[2]),
      A = bR(n.marginLeft, q[3]),
      E = n.spacingTop,
      F = n.spacingRight,
      Q = n.spacingBottom,
      T = n.spacingLeft,
      U, V, W, X, Y, Z, $, _, ba, bb, bc, bd, be, bf, bg, bh, bi, bj, bk, bl, bm, bn, bo = this,
      bp = n.events,
      bq = bp && !!bp.click,
      br, bs, bw, bF, bJ, bK, bU, cd, ce, cf, cg, ch, ci, ck, cr, cs, ct = n.showAxes,
      cu = 0,
      cw = [],
      cx, cy = [],
      cz, cA, cB, cC, cD, cE, cF, cG, cH, cI, cJ, cK;
    var cP = function () {
      function H() {
        n = m;
        o = k + r + l - 5;
        x = 0;
        p = 0;
        if (!w) {
          w = cA.g("legend").attr({
            zIndex: 10
          }).add()
        }
        e = [];
        bt(z, function (a) {
          var b = a.options;
          if (!b.showInLegend) {
            return
          }
          e = e.concat(a.legendItems || (b.legendType === "point" ? a.data : a))
        });
        b$(e, function (a, b) {
          return (a.options.legendIndex || 0) - (b.options.legendIndex || 0)
        });
        if (A) {
          e.reverse()
        }
        bt(e, G);
        ck = y || x;
        cr = p - l + q;
        if (u || v) {
          ck += 2 * k;
          cr += 2 * k;
          if (!t) {
            t = cA.rect(0, 0, ck, cr, a.borderRadius, u || 0).attr({
              stroke: a.borderColor,
              "stroke-width": u || 0,
              fill: v || N
            }).add(w).shadow(a.shadow);
            t.isNew = true
          } else if (ck > 0 && cr > 0) {
            t[t.isNew ? "attr" : "animate"](t.crisp(null, null, null, ck, cr));
            t.isNew = false
          }
          t[e.length ? "show" : "hide"]()
        }
        var b = ["left", "right", "top", "bottom"],
          c, d = 4;
        while (d--) {
          c = b[d];
          if (f[c] && f[c] !== "auto") {
            a[d < 2 ? "align" : "verticalAlign"] = c;
            a[d < 2 ? "x" : "y"] = bG(f[c]) * (d % 2 ? -1 : 1)
          }
        }
        if (e.length) {
          w.align(bE(a, {
            width: ck,
            height: cr
          }), true, U)
        }
        if (!cu) {
          F()
        }
      }

      function G(e) {
        var f, l, t, u, v, z, A, D = e.legendItem,
          E = e.series || e,
          F = E.options,
          G = F && F.borderWidth || 0;
        if (!D) {
          z = /^(bar|pie|area|column)$/.test(E.type);
          e.legendItem = D = cA.text(a.labelFormatter.call(e), 0, 0).css(e.visible ? g : j).on("mouseover", function () {
            e.setState(S);
            D.css(i)
          }).on("mouseout", function () {
            D.css(e.visible ? g : j);
            e.setState()
          }).on("click", function () {
            var a = "legendItemClick",
              b = function () {
                e.setVisible()
              };
            if (e.firePointEvent) {
              e.firePointEvent(a, null, b)
            } else {
              bA(e, a, null, b)
            }
          }).attr({
            zIndex: 2
          }).add(w);
          if (!z && F && F.lineWidth) {
            var H = {
              "stroke-width": F.lineWidth,
              zIndex: 2
            };
            if (F.dashStyle) {
              H.dashstyle = F.dashStyle
            }
            e.legendLine = cA.path([O, -c - d, 0, P, -d, 0]).attr(H).add(w)
          }
          if (z) {
            t = cA.rect(u = -c - d, v = -11, c, 12, 2).attr({
              zIndex: 3
            }).add(w)
          } else if (F && F.marker && F.marker.enabled) {
            A = F.marker.radius;
            t = cA.symbol(e.symbol, u = -c / 2 - d - A, v = -4 - A, 2 * A, 2 * A).attr(e.pointAttr[R]).attr({
              zIndex: 3
            }).add(w)
          }
          if (t) {
            t.xOff = u + G % 2 / 2;
            t.yOff = v + G % 2 / 2
          }
          e.legendSymbol = t;
          B(e, e.visible);
          if (F && F.showCheckbox) {
            e.checkbox = bT("input", {
              type: "checkbox",
              checked: e.selected,
              defaultChecked: e.selected
            }, a.itemCheckboxStyle, bc);
            by(e.checkbox, "click", function (a) {
              var b = a.target;
              bA(e, "checkboxClick", {
                checked: b.checked
              }, function () {
                e.select()
              })
            })
          }
        }
        f = D.getBBox();
        l = e.legendItemWidth = a.itemWidth || c + d + f.width + k;
        q = f.height;
        if (b && n - m + l > (y || bg - 2 * k - m)) {
          n = m;
          o += r + q + s
        }
        p = o + s;
        C(e, n, o);
        if (b) {
          n += l
        } else {
          o += r + q + s
        }
        x = y || h(b ? n - m : l, x)
      }

      function F() {
        bt(e, function (a) {
          var b = a.checkbox,
            c = w.alignAttr;
          if (b) {
            bS(b, {
              left: c.translateX + a.legendItemWidth + b.x - 40 + M,
              top: c.translateY + b.y - 11 + M
            })
          }
        })
      }

      function E() {
        if (t) {
          t = t.destroy()
        }
        if (w) {
          w = w.destroy()
        }
      }

      function D(a) {
        var b = a.checkbox;
        bt(["legendItem", "legendLine", "legendSymbol"], function (b) {
          if (a[b]) {
            a[b].destroy()
          }
        });
        if (b) {
          cj(a.checkbox)
        }
      }

      function C(a, b, c) {
        var d = a.legendItem,
          e = a.legendLine,
          f = a.legendSymbol,
          g = a.checkbox;
        if (d) {
          d.attr({
            x: b,
            y: c
          })
        }
        if (e) {
          e.translate(b, c - 4)
        }
        if (f) {
          f.attr({
            x: b + f.xOff,
            y: c + f.yOff
          })
        }
        if (g) {
          g.x = b;
          g.y = c
        }
      }

      function B(b, c) {
        var d = b.legendItem,
          e = b.legendLine,
          f = b.legendSymbol,
          g = j.color,
          h = c ? a.itemStyle.color : g,
          i = c ? b.color : g;
        if (d) {
          d.css({
            fill: h
          })
        }
        if (e) {
          e.attr({
            stroke: i
          })
        }
        if (f) {
          f.attr({
            stroke: i,
            fill: i
          })
        }
      }
      var a = bo.options.legend;
      if (!a.enabled) {
        return
      }
      var b = a.layout === "horizontal",
        c = a.symbolWidth,
        d = a.symbolPadding,
        e, f = a.style,
        g = a.itemStyle,
        i = a.itemHoverStyle,
        j = bx(g, a.itemHiddenStyle),
        k = a.padding || bG(f.padding),
        l = 18,
        m = 4 + k + c + d,
        n, o, p, q = 0,
        r = a.itemMarginTop || 0,
        s = a.itemMarginBottom || 0,
        t, u = a.borderWidth,
        v = a.backgroundColor,
        w, x, y = a.width,
        z = bo.series,
        A = a.reversed;
      H();
      by(bo, "endResize", F);
      return {
        colorizeItem: B,
        destroyItem: D,
        renderLegend: H,
        destroy: E
      }
    };
    bs = function (a, b) {
      return a >= 0 && a <= ce && b >= 0 && b <= cd
    };
    cK = function () {
      bA(bo, "selection", {
        resetSelection: true
      }, cJ);
      bo.toolbar.remove("zoom")
    };
    cJ = function (a) {
      var b = B.lang,
        c = bo.pointCount < 100;
      if (bo.resetZoomEnabled !== false) {
        bo.toolbar.add("zoom", b.resetZoom, b.resetZoomTitle, cK)
      }
      if (!a || a.resetSelection) {
        bt(cw, function (a) {
          if (a.options.zoomEnabled !== false) {
            a.setExtremes(null, null, true, c)
          }
        })
      } else {
        bt(a.xAxis.concat(a.yAxis), function (a) {
          var b = a.axis;
          if (bo.tracker[b.isXAxis ? "zoomX" : "zoomY"]) {
            b.setExtremes(a.min, a.max, true, c)
          }
        })
      }
    };
    bo.pan = function (a) {
      var b = bo.xAxis[0],
        c = bo.mouseDownX,
        d = b.pointRange / 2,
        e = b.getExtremes(),
        f = b.translate(c - a, true) + d,
        g = b.translate(c + ce - a, true) - d,
        j = bo.hoverPoints;
      if (j) {
        bt(j, function (a) {
          a.setState()
        })
      }
      if (f > i(e.dataMin, e.min) && g < h(e.dataMax, e.max)) {
        b.setExtremes(f, g, true, false)
      }
      bo.mouseDownX = a;
      bS(bc, {
        cursor: "move"
      })
    };
    cF = function () {
      var a = k.legend,
        b = bR(a.margin, 10),
        c = a.x,
        d = a.y,
        e = a.align,
        f = a.verticalAlign,
        g;
      cG();
      if ((bo.title || bo.subtitle) && !bO(t)) {
        g = h(bo.title && !V.floating && !V.verticalAlign && V.y || 0, bo.subtitle && !W.floating && !W.verticalAlign && W.y || 0);
        if (g) {
          X = h(X, g + bR(V.margin, 15) + E)
        }
      }
      if (a.enabled && !a.floating) {
        if (e === "right") {
          if (!bO(v)) {
            Y = h(Y, ck - c + b + F)
          }
        } else if (e === "left") {
          if (!bO(A)) {
            $ = h($, ck + c + b + T)
          }
        } else if (f === "top") {
          if (!bO(t)) {
            X = h(X, cr + d + b + E)
          }
        } else if (f === "bottom") {
          if (!bO(y)) {
            Z = h(Z, cr - d + b + Q)
          }
        }
      }
      if (bo.extraBottomMargin) {
        Z += bo.extraBottomMargin
      }
      if (bo.extraTopMargin) {
        X += bo.extraTopMargin
      }
      if (ct) {
        bt(cw, function (a) {
          a.getOffset()
        })
      }
      if (!bO(A)) {
        $ += _[3]
      }
      if (!bO(t)) {
        X += _[0]
      }
      if (!bO(y)) {
        Z += _[2]
      }
      if (!bO(v)) {
        Y += _[1]
      }
      cH()
    };
    cI = function (a, b, c) {
      var d = bo.title,
        f = bo.subtitle;
      cu += 1;
      cc(c, bo);
      bj = bh;
      bi = bg;
      if (bO(a)) {
        bo.chartWidth = bg = e(a)
      }
      if (bO(b)) {
        bo.chartHeight = bh = e(b)
      }
      bS(bc, {
        width: bg + M,
        height: bh + M
      });
      cA.setSize(bg, bh, c);
      ce = bg - $ - Y;
      cd = bh - X - Z;
      cx = null;
      bt(cw, function (a) {
        a.isDirty = true;
        a.setScale()
      });
      bt(cy, function (a) {
        a.isDirty = true
      });
      bo.isDirtyLegend = true;
      bo.isDirtyBox = true;
      cF();
      if (d) {
        d.align(null, null, U)
      }
      if (f) {
        f.align(null, null, U)
      }
      cT(c);
      bj = null;
      bA(bo, "resize");
      if (D === false) {
        dc()
      } else {
        setTimeout(dc, D && D.duration || 500)
      }
    };
    cH = function () {
      bo.plotLeft = $ = e($);
      bo.plotTop = X = e(X);
      bo.plotWidth = ce = e(bg - $ - Y);
      bo.plotHeight = cd = e(bh - X - Z);
      bo.plotSizeX = cz ? cd : ce;
      bo.plotSizeY = cz ? ce : cd;
      U = {
        x: T,
        y: E,
        width: bg - T - F,
        height: bh - E - Q
      };
      bt(cw, function (a) {
        if (a.isDirty) {
          a.setAxisSize()
        }
      })
    };
    cG = function () {
      X = bR(t, E);
      Y = bR(v, F);
      Z = bR(y, Q);
      $ = bR(A, T);
      _ = [0, 0, 0, 0]
    };
    cE = function () {
      var a = n.borderWidth || 0,
        b = n.backgroundColor,
        c = n.plotBackgroundColor,
        d = n.plotBackgroundImage,
        e, f = {
          x: $,
          y: X,
          width: ce,
          height: cd
        };
      e = a + (n.shadow ? 8 : 0);
      if (a || b) {
        if (!bk) {
          bk = cA.rect(e / 2, e / 2, bg - e, bh - e, n.borderRadius, a).attr({
            stroke: n.borderColor,
            "stroke-width": a,
            fill: b || N
          }).add().shadow(n.shadow)
        } else {
          bk.animate(bk.crisp(null, null, null, bg - e, bh - e))
        }
      }
      if (c) {
        if (!bl) {
          bl = cA.rect($, X, ce, cd, 0).attr({
            fill: c
          }).add().shadow(n.plotShadow)
        } else {
          bl.animate(f)
        }
      }
      if (d) {
        if (!bm) {
          bm = cA.image(d, $, X, ce, cd).add()
        } else {
          bm.animate(f)
        }
      }
      if (n.plotBorderWidth) {
        if (!bn) {
          bn = cA.rect($, X, ce, cd, 0, n.plotBorderWidth).attr({
            stroke: n.plotBorderColor,
            "stroke-width": n.plotBorderWidth,
            zIndex: 4
          }).add()
        } else {
          bn.animate(bn.crisp(null, $, X, ce, cd))
        }
      }
      bo.isDirtyBox = false
    };
    if (n.reflow !== false) {
      by(bo, "load", db)
    }
    if (bp) {
      for (br in bp) {
        by(bo, br, bp[br])
      }
    }
    bo.options = k;
    bo.series = cy;
    bo.xAxis = [];
    bo.yAxis = [];
    bo.addSeries = cR;
    bo.animation = bR(n.animation, true);
    bo.Axis = cL;
    bo.destroy = df;
    bo.get = cW;
    bo.getSelectedPoints = cY;
    bo.getSelectedSeries = cZ;
    bo.hideLoading = cV;
    bo.initSeries = cQ;
    bo.isInsidePlot = bs;
    bo.redraw = cT;
    bo.setSize = cI;
    bo.setTitle = c$;
    bo.showLoading = cU;
    bo.pointCount = 0;
    bo.counters = new bY;
    dg()
  }

  function cu() { }

  function cj(a) {
    if (!A) {
      A = bT(G)
    }
    if (a) {
      A.appendChild(a)
    }
    A.innerHTML = ""
  }

  function ci() {
    return B
  }

  function ch(b) {
    cl = bx(cl, b.xAxis);
    cm = bx(cm, b.yAxis);
    b.xAxis = b.yAxis = a;
    B = bx(B, b);
    cg();
    return B
  }

  function cg() {
    var a = B.global.useUTC;
    bf = a ? Date.UTC : function (a, b, c, d, e, f) {
      return (new Date(a, b, bR(c, 1), bR(d, 0), bR(e, 0), bR(f, 0))).getTime()
    };
    bg = a ? "getUTCMinutes" : "getMinutes";
    bh = a ? "getUTCHours" : "getHours";
    bi = a ? "getUTCDay" : "getDay";
    bj = a ? "getUTCDate" : "getDate";
    bk = a ? "getUTCMonth" : "getMonth";
    bl = a ? "getUTCFullYear" : "getFullYear";
    bm = a ? "setUTCMinutes" : "setMinutes";
    bn = a ? "setUTCHours" : "setHours";
    bo = a ? "setUTCDate" : "setDate";
    bp = a ? "setUTCMonth" : "setMonth";
    bq = a ? "setUTCFullYear" : "setFullYear"
  }

  function cc(a, b) {
    D = bR(a, b.animation)
  }

  function cb(a) {
    var b;
    for (b in a) {
      if (a[b] && a[b].destroy) {
        a[b].destroy()
      }
      delete a[b]
    }
  }

  function ca(a) {
    var b = a.length,
      c = a[0];
    while (b--) {
      if (a[b] > c) {
        c = a[b]
      }
    }
    return c
  }

  function b_(a) {
    var b = a.length,
      c = a[0];
    while (b--) {
      if (a[b] < c) {
        c = a[b]
      }
    }
    return c
  }

  function b$(a, b) {
    var c = a.length,
      d, e;
    for (e = 0; e < c; e++) {
      a[e].ss_i = e
    }
    a.sort(function (a, c) {
      d = b(a, c);
      return d === 0 ? a.ss_i - c.ss_i : d
    });
    for (e = 0; e < c; e++) {
      delete a[e].ss_i
    }
  }

  function bZ(a, b, c, d, e, f, g, h) {
    var i = g.x,
      j = g.y,
      k = i - a + c - h,
      l = j - b + d + 15,
      m;
    if (k < 7) {
      k = c + i + h
    }
    if (k + a > c + e) {
      k -= k + a - (c + e);
      l = j - b + d - h;
      m = true
    }
    if (l < d + 5) {
      l = d + 5;
      if (m && j >= l && j <= l + b) {
        l = j + d + h
      }
    } else if (l + b > d + f) {
      l = d + f - b - h
    }
    return {
      x: k,
      y: l
    }
  }

  function bY() {
    this.color = 0;
    this.symbol = 0
  }

  function bX(a, b, c, d, e) {
    var g = [],
      h, i = {},
      j = B.global.useUTC,
      k = e || [
        [U, [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]],
        [V, [1, 2, 5, 10, 15, 30]],
        [W, [1, 2, 5, 10, 15, 30]],
        [X, [1, 2, 3, 4, 6, 8, 12]],
        [Y, [1, 2]],
        [Z, [1, 2]],
        [$, [1, 2, 3, 4, 6]],
        [_, null]
      ],
      l = k[k.length - 1],
      m = F[l[0]],
      n = l[1];
    for (h = 0; h < k.length; h++) {
      l = k[h];
      m = F[l[0]];
      n = l[1];
      if (k[h + 1]) {
        var o = (m * n[n.length - 1] + F[k[h + 1][0]]) / 2;
        if (a <= o) {
          break
        }
      }
    }
    if (m === F[_] && a < 5 * m) {
      n = [1, 2, 5]
    }
    var p = bW(a / m, n),
      q, r = new Date(b);
    r.setMilliseconds(0);
    if (m >= F[V]) {
      r.setSeconds(m >= F[W] ? 0 : p * f(r.getSeconds() / p))
    }
    if (m >= F[W]) {
      r[bm](m >= F[X] ? 0 : p * f(r[bg]() / p))
    }
    if (m >= F[X]) {
      r[bn](m >= F[Y] ? 0 : p * f(r[bh]() / p))
    }
    if (m >= F[Y]) {
      r[bo](m >= F[$] ? 1 : p * f(r[bj]() / p))
    }
    if (m >= F[$]) {
      r[bp](m >= F[_] ? 0 : p * f(r[bk]() / p));
      q = r[bl]()
    }
    if (m >= F[_]) {
      q -= q % p;
      r[bq](q)
    }
    if (m === F[Z]) {
      r[bo](r[bj]() - r[bi]() + bR(d, 1))
    }
    h = 1;
    q = r[bl]();
    var s = r.getTime(),
      t = r[bk](),
      u = r[bj]();
    while (s < c) {
      g.push(s);
      if (m === F[_]) {
        s = bf(q + h * p, 0)
      } else if (m === F[$]) {
        s = bf(q, t + h * p)
      } else if (!j && (m === F[Y] || m === F[Z])) {
        s = bf(q, t, u + h * p * (m === F[Y] ? 1 : 7))
      } else {
        s += m * p;
        if (m <= F[X] && s % F[Y] === 0) {
          i[s] = Y
        }
      }
      h++
    }
    g.push(s);
    g.info = {
      unitName: l[0],
      unitRange: m,
      count: p,
      higherRanks: i,
      totalRange: m * p
    };
    return g
  }

  function bW(a, b, c, d) {
    var e, f;
    c = bR(c, 1);
    e = a / c;
    if (!b) {
      b = [1, 2, 2.5, 5, 10];
      if (d && (d.allowDecimals === false || d.type === "logarithmic")) {
        if (c === 1) {
          b = [1, 2, 5, 10]
        } else if (c <= .1) {
          b = [1 / c]
        }
      }
    }
    for (f = 0; f < b.length; f++) {
      a = b[f];
      if (e <= (b[f] + (b[f + 1] || b[f])) / 2) {
        break
      }
    }
    a *= c;
    return a
  }

  function bV(a, b, c, d) {
    var e = B.lang,
      f = a,
      g = isNaN(b = j(b)) ? 2 : b,
      h = c === undefined ? e.decimalPoint : c,
      i = d === undefined ? e.thousandsSep : d,
      k = f < 0 ? "-" : "",
      l = String(bG(f = j(+f || 0).toFixed(g))),
      m = l.length > 3 ? l.length % 3 : 0;
    return k + (m ? l.substr(0, m) + i : "") + l.substr(m).replace(/(\d{3})(?=\d)/g, "$1" + i) + (g ? h + j(f - l).toFixed(g).slice(2) : "")
  }

  function bU(a, b) {
    var c = function () { };
    c.prototype = new a;
    bE(c.prototype, b);
    return c
  }

  function bT(a, c, d, e, f) {
    var g = b.createElement(a);
    if (c) {
      bE(g, c)
    }
    if (f) {
      bS(g, {
        padding: 0,
        border: N,
        margin: 0
      })
    }
    if (d) {
      bS(g, d)
    }
    if (e) {
      e.appendChild(g)
    }
    return g
  }

  function bS(b, c) {
    if (p) {
      if (c && c.opacity !== a) {
        c.filter = "alpha(opacity=" + c.opacity * 100 + ")"
      }
    }
    bE(b.style, c)
  }

  function bR() {
    var a = arguments,
      b, c, d = a.length;
    for (b = 0; b < d; b++) {
      c = a[b];
      if (typeof c !== "undefined" && c !== null) {
        return c
      }
    }
  }

  function bQ(a) {
    return bJ(a) ? a : [a]
  }

  function bP(a, b, c) {
    var d, e = "setAttribute",
      f;
    if (bH(b)) {
      if (bO(c)) {
        a[e](b, c)
      } else if (a && a.getAttribute) {
        f = a.getAttribute(b)
      }
    } else if (bO(b) && bI(b)) {
      for (d in b) {
        a[e](d, b[d])
      }
    }
    return f
  }

  function bO(b) {
    return b !== a && b !== null
  }

  function bN(a, b) {
    var c = a.length;
    while (c--) {
      if (a[c] === b) {
        a.splice(c, 1);
        break
      }
    }
  }

  function bM(a) {
    return d.pow(10, a)
  }

  function bL(a) {
    return d.log(a) / d.LN10
  }

  function bK(a) {
    return typeof a === "number"
  }

  function bJ(a) {
    return Object.prototype.toString.call(a) === "[object Array]"
  }

  function bI(a) {
    return typeof a === "object"
  }

  function bH(a) {
    return typeof a === "string"
  }

  function bG(a, b) {
    return parseInt(a, b || 10)
  }

  function bF() {
    var a = 0,
      b = arguments,
      c = b.length,
      d = {};
    for (; a < c; a++) {
      d[b[a++]] = b[a]
    }
    return d
  }

  function bE(a, b) {
    var c;
    if (!a) {
      a = {}
    }
    for (c in b) {
      a[c] = b[c]
    }
    return a
  }
  var a, b = document,
    c = window,
    d = Math,
    e = d.round,
    f = d.floor,
    g = d.ceil,
    h = d.max,
    i = d.min,
    j = d.abs,
    k = d.cos,
    l = d.sin,
    m = d.PI,
    n = m * 2 / 360,
    o = navigator.userAgent,
    p = /msie/i.test(o) && !c.opera,
    q = b.documentMode === 8,
    r = /AppleWebKit/.test(o),
    s = /Firefox/.test(o),
    t = "http://www.w3.org/2000/svg",
    u = !!b.createElementNS && !!b.createElementNS(t, "svg").createSVGRect,
    v = s && parseInt(o.split("Firefox/")[1], 10) < 4,
    w, x = b.documentElement.ontouchstart !== a,
    y = {},
    z = 0,
    A, B, C, D, E, F, G = "div",
    H = "absolute",
    I = "relative",
    J = "hidden",
    K = "highcharts-",
    L = "visible",
    M = "px",
    N = "none",
    O = "M",
    P = "L",
    Q = "rgba(192,192,192," + (u ? 1e-6 : .002) + ")",
    R = "",
    S = "hover",
    T = "select",
    U = "millisecond",
    V = "second",
    W = "minute",
    X = "hour",
    Y = "day",
    Z = "week",
    $ = "month",
    _ = "year",
    ba = "fill",
    bb = "linearGradient",
    bc = "stops",
    bd = "stroke",
    be = "stroke-width",
    bf, bg, bh, bi, bj, bk, bl, bm, bn, bo, bp, bq, br = c.HighchartsAdapter,
    bs = br || {},
    bt = bs.each,
    bu = bs.grep,
    bv = bs.offset,
    bw = bs.map,
    bx = bs.merge,
    by = bs.addEvent,
    bz = bs.removeEvent,
    bA = bs.fireEvent,
    bB = bs.animate,
    bC = bs.stop,
    bD = {};
  c.Highcharts = {};
  C = function (a, b, c) {
    function d(a, b) {
      a = a.toString().replace(/^([0-9])$/, "0$1");
      if (b === 3) {
        a = a.toString().replace(/^([0-9]{2})$/, "0$1")
      }
      return a
    }
    if (!bO(b) || isNaN(b)) {
      return "Invalid date"
    }
    a = bR(a, "%Y-%m-%d %H:%M:%S");
    var e = new Date(b),
      f, g = e[bh](),
      h = e[bi](),
      i = e[bj](),
      j = e[bk](),
      k = e[bl](),
      l = B.lang,
      m = l.weekdays,
      n = {
        a: m[h].substr(0, 3),
        A: m[h],
        d: d(i),
        e: i,
        b: l.shortMonths[j],
        B: l.months[j],
        m: d(j + 1),
        y: k.toString().substr(2, 2),
        Y: k,
        H: d(g),
        I: d(g % 12 || 12),
        l: g % 12 || 12,
        M: d(e[bg]()),
        p: g < 12 ? "AM" : "PM",
        P: g < 12 ? "am" : "pm",
        S: d(e.getSeconds()),
        L: d(b % 1e3, 3)
      };
    for (f in n) {
      a = a.replace("%" + f, n[f])
    }
    return c ? a.substr(0, 1).toUpperCase() + a.substr(1) : a
  };
  bY.prototype = {
    wrapColor: function (a) {
      if (this.color >= a) {
        this.color = 0
      }
    },
    wrapSymbol: function (a) {
      if (this.symbol >= a) {
        this.symbol = 0
      }
    }
  };
  F = bF(U, 1, V, 1e3, W, 6e4, X, 36e5, Y, 24 * 36e5, Z, 7 * 24 * 36e5, $, 30 * 24 * 36e5, _, 31556952e3);
  E = {
    init: function (a, b, c) {
      b = b || "";
      var d = a.shift,
        e = b.indexOf("C") > -1,
        f = e ? 7 : 3,
        g, h, i, j = b.split(" "),
        k = [].concat(c),
        l, m, n = function (a) {
          i = a.length;
          while (i--) {
            if (a[i] === O) {
              a.splice(i + 1, 0, a[i + 1], a[i + 2], a[i + 1], a[i + 2])
            }
          }
        };
      if (e) {
        n(j);
        n(k)
      }
      if (a.isArea) {
        l = j.splice(j.length - 6, 6);
        m = k.splice(k.length - 6, 6)
      }
      if (d === 1) {
        k = [].concat(k).splice(0, f).concat(k)
      }
      a.shift = 0;
      if (j.length) {
        g = k.length;
        while (j.length < g) {
          h = [].concat(j).splice(j.length - f, f);
          if (e) {
            h[f - 6] = h[f - 2];
            h[f - 5] = h[f - 1]
          }
          j = j.concat(h)
        }
      }
      if (l) {
        j = j.concat(l);
        k = k.concat(m)
      }
      return [j, k]
    },
    step: function (a, b, c, d) {
      var e = [],
        f = a.length,
        g;
      if (c === 1) {
        e = d
      } else if (f === b.length && c < 1) {
        while (f--) {
          g = parseFloat(a[f]);
          e[f] = isNaN(g) ? a[f] : c * parseFloat(b[f] - g) + g
        }
      } else {
        e = b
      }
      return e
    }
  };
  if (br && br.init) {
    br.init(E)
  }
  if (!br && c.jQuery) {
    var cd = jQuery;
    bt = function (a, b) {
      var c = 0,
        d = a.length;
      for (; c < d; c++) {
        if (b.call(a[c], a[c], c, a) === false) {
          return c
        }
      }
    };
    bu = cd.grep;
    bw = function (a, b) {
      var c = [],
        d = 0,
        e = a.length;
      for (; d < e; d++) {
        c[d] = b.call(a[d], a[d], d, a)
      }
      return c
    };
    bx = function () {
      var a = arguments;
      return cd.extend(true, null, a[0], a[1], a[2], a[3])
    };
    bv = function (a) {
      return cd(a).offset()
    };
    by = function (a, b, c) {
      cd(a).bind(b, c)
    };
    bz = function (a, c, d) {
      var e = b.removeEventListener ? "removeEventListener" : "detachEvent";
      if (b[e] && !a[e]) {
        a[e] = function () { }
      }
      cd(a).unbind(c, d)
    };
    bA = function (a, b, c, d) {
      var e = cd.Event(b),
        f = "detached" + b;
      bE(e, c);
      if (a[b]) {
        a[f] = a[b];
        a[b] = null
      }
      cd(a).trigger(e);
      if (a[f]) {
        a[b] = a[f];
        a[f] = null
      }
      if (d && !e.isDefaultPrevented()) {
        d(e)
      }
    };
    bB = function (a, b, c) {
      var d = cd(a);
      if (b.d) {
        a.toD = b.d;
        b.d = 1
      }
      d.stop();
      d.animate(b, c)
    };
    bC = function (a) {
      cd(a).stop()
    };
    cd.extend(cd.easing, {
      easeOutQuad: function (a, b, c, d, e) {
        return -d * (b /= e) * (b - 2) + c
      }
    });
    var ce = jQuery.fx,
      cf = ce.step;
    bt(["cur", "_default", "width", "height"], function (a, b) {
      var c = b ? cf : ce.prototype,
        d = c[a],
        e;
      if (d) {
        c[a] = function (a) {
          a = b ? a : this;
          e = a.elem;
          return e.attr ? e.attr(a.prop, a.now) : d.apply(this, arguments)
        }
      }
    });
    cf.d = function (a) {
      var b = a.elem;
      if (!a.started) {
        var c = E.init(b, b.d, b.toD);
        a.start = c[0];
        a.end = c[1];
        a.started = true
      }
      b.attr("d", E.step(a.start, a.end, a.pos, b.toD))
    }
  }
  var ck = {
    enabled: true,
    align: "center",
    x: 0,
    y: 15,
    style: {
      color: "#666",
      fontSize: "11px",
      lineHeight: "14px"
    }
  };
  B = {
    colors: ["#4572A7", "#AA4643", "#89A54E", "#80699B", "#3D96AE", "#DB843D", "#92A8CD", "#A47D7C", "#B5CA92"],
    symbols: ["circle", "diamond", "square", "triangle", "triangle-down"],
    lang: {
      loading: "Loading...",
      months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
      shortMonths: ["Jan", "Feb", "Mar", "Apr", "May", "June", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
      weekdays: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
      decimalPoint: ".",
      resetZoom: "Reset zoom",
      resetZoomTitle: "Reset zoom level 1:1",
      thousandsSep: ","
    },
    global: {
      useUTC: true
    },
    chart: {
      borderColor: "#4572A7",
      borderRadius: 5,
      defaultSeriesType: "line",
      ignoreHiddenSeries: true,
      spacingTop: 10,
      spacingRight: 10,
      spacingBottom: 15,
      spacingLeft: 10,
      style: {
        fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Verdana, Arial, Helvetica, sans-serif',
        fontSize: "12px"
      },
      backgroundColor: "#FFFFFF",
      plotBorderColor: "#C0C0C0"
    },
    title: {
      text: "Chart title",
      align: "center",
      y: 15,
      style: {
        color: "#3E576F",
        fontSize: "16px"
      }
    },
    subtitle: {
      text: "",
      align: "center",
      y: 30,
      style: {
        color: "#6D869F"
      }
    },
    plotOptions: {
      line: {
        allowPointSelect: false,
        showCheckbox: false,
        animation: {
          duration: 1e3
        },
        events: {},
        lineWidth: 2,
        shadow: true,
        marker: {
          enabled: true,
          lineWidth: 0,
          radius: 4,
          lineColor: "#FFFFFF",
          states: {
            hover: {},
            select: {
              fillColor: "#FFFFFF",
              lineColor: "#000000",
              lineWidth: 2
            }
          }
        },
        point: {
          events: {}
        },
        dataLabels: bx(ck, {
          enabled: false,
          y: -6,
          formatter: function () {
            return this.y
          }
        }),
        cropThreshold: 300,
        pointRange: 0,
        showInLegend: true,
        states: {
          hover: {
            marker: {}
          },
          select: {
            marker: {}
          }
        },
        stickyTracking: true
      }
    },
    labels: {
      style: {
        position: H,
        color: "#3E576F"
      }
    },
    legend: {
      enabled: true,
      align: "center",
      layout: "horizontal",
      labelFormatter: function () {
        return this.name
      },
      borderWidth: 1,
      borderColor: "#909090",
      borderRadius: 5,
      shadow: false,
      style: {
        padding: "5px"
      },
      itemStyle: {
        cursor: "pointer",
        color: "#3E576F"
      },
      itemHoverStyle: {
        color: "#000000"
      },
      itemHiddenStyle: {
        color: "#C0C0C0"
      },
      itemCheckboxStyle: {
        position: H,
        width: "13px",
        height: "13px"
      },
      symbolWidth: 16,
      symbolPadding: 5,
      verticalAlign: "bottom",
      x: 0,
      y: 0
    },
    loading: {
      labelStyle: {
        fontWeight: "bold",
        position: I,
        top: "1em"
      },
      style: {
        position: H,
        backgroundColor: "white",
        opacity: .5,
        textAlign: "center"
      }
    },
    tooltip: {
      enabled: true,
      backgroundColor: "rgba(255, 255, 255, .85)",
      borderWidth: 2,
      borderRadius: 5,
      headerFormat: '<span style="font-size: 10px">{point.key}</span><br/>',
      pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
      shadow: true,
      snap: x ? 25 : 10,
      style: {
        color: "#333333",
        fontSize: "12px",
        padding: "5px",
        whiteSpace: "nowrap"
      }
    },
    toolbar: {
      itemStyle: {
        color: "#4572A7",
        cursor: "pointer"
      }
    },
    credits: {
      enabled: true,
      text: "Highcharts.com",
      href: "http://www.highcharts.com",
      position: {
        align: "right",
        x: -10,
        verticalAlign: "bottom",
        y: -5
      },
      style: {
        cursor: "pointer",
        color: "#909090",
        fontSize: "10px"
      }
    }
  };
  var cl = {
    dateTimeLabelFormats: bF(U, "%H:%M:%S.%L", V, "%H:%M:%S", W, "%H:%M", X, "%H:%M", Y, "%e. %b", Z, "%e. %b", $, "%b '%y", _, "%Y"),
    endOnTick: false,
    gridLineColor: "#C0C0C0",
    labels: ck,
    lineColor: "#C0D0E0",
    lineWidth: 1,
    max: null,
    min: null,
    minPadding: .01,
    maxPadding: .01,
    minorGridLineColor: "#E0E0E0",
    minorGridLineWidth: 1,
    minorTickColor: "#A0A0A0",
    minorTickLength: 2,
    minorTickPosition: "outside",
    startOfWeek: 1,
    startOnTick: false,
    tickColor: "#C0D0E0",
    tickLength: 5,
    tickmarkPlacement: "between",
    tickPixelInterval: 100,
    tickPosition: "outside",
    tickWidth: 1,
    title: {
      align: "middle",
      style: {
        color: "#6D869F",
        fontWeight: "bold"
      }
    },
    type: "linear"
  },
    cm = bx(cl, {
      endOnTick: true,
      gridLineWidth: 1,
      tickPixelInterval: 72,
      showLastLabel: true,
      labels: {
        align: "right",
        x: -8,
        y: 3
      },
      lineWidth: 0,
      maxPadding: .05,
      minPadding: .05,
      startOnTick: true,
      tickWidth: 0,
      title: {
        rotation: 270,
        text: "Y-values"
      },
      stackLabels: {
        enabled: false,
        formatter: function () {
          return this.total
        },
        style: ck.style
      }
    }),
    cn = {
      labels: {
        align: "right",
        x: -8,
        y: null
      },
      title: {
        rotation: 270
      }
    },
    co = {
      labels: {
        align: "left",
        x: 8,
        y: null
      },
      title: {
        rotation: 90
      }
    },
    cp = {
      labels: {
        align: "center",
        x: 0,
        y: 14
      },
      title: {
        rotation: 0
      }
    },
    cq = bx(cp, {
      labels: {
        y: -5
      }
    });
  var cr = B.plotOptions,
    cs = cr.line;
  cr.spline = bx(cs);
  cr.scatter = bx(cs, {
    lineWidth: 0,
    states: {
      hover: {
        lineWidth: 0
      }
    },
    tooltip: {
      headerFormat: '<span style="font-size: 10px; color:{series.color}">{series.name}</span><br/>',
      pointFormat: "x: <b>{point.x}</b><br/>y: <b>{point.y}</b><br/>"
    }
  });
  cr.area = bx(cs, {
    threshold: 0
  });
  cr.areaspline = bx(cr.area);
  cr.column = bx(cs, {
    borderColor: "#FFFFFF",
    borderWidth: 1,
    borderRadius: 0,
    groupPadding: .2,
    marker: null,
    pointPadding: .1,
    minPointLength: 0,
    cropThreshold: 50,
    pointRange: null,
    states: {
      hover: {
        brightness: .1,
        shadow: false
      },
      select: {
        color: "#C0C0C0",
        borderColor: "#000000",
        shadow: false
      }
    },
    dataLabels: {
      y: null,
      verticalAlign: null
    },
    threshold: 0
  });
  cr.bar = bx(cr.column, {
    dataLabels: {
      align: "left",
      x: 5,
      y: 0
    }
  });
  cr.pie = bx(cs, {
    borderColor: "#FFFFFF",
    borderWidth: 1,
    center: ["50%", "50%"],
    colorByPoint: true,
    dataLabels: {
      distance: 30,
      enabled: true,
      formatter: function () {
        return this.point.name
      },
      y: 5
    },
    legendType: "point",
    marker: null,
    size: "75%",
    showInLegend: false,
    slicedOffset: 10,
    states: {
      hover: {
        brightness: .1,
        shadow: false
      }
    }
  });
  cg();
  var ct = function (a) {
    function g(a) {
      b[3] = a;
      return this
    }

    function f(a) {
      if (bK(a) && a !== 0) {
        var c;
        for (c = 0; c < 3; c++) {
          b[c] += bG(a * 255);
          if (b[c] < 0) {
            b[c] = 0
          }
          if (b[c] > 255) {
            b[c] = 255
          }
        }
      }
      return this
    }

    function e(c) {
      var d;
      if (b && !isNaN(b[0])) {
        if (c === "rgb") {
          d = "rgb(" + b[0] + "," + b[1] + "," + b[2] + ")"
        } else if (c === "a") {
          d = b[3]
        } else {
          d = "rgba(" + b.join(",") + ")"
        }
      } else {
        d = a
      }
      return d
    }

    function d(a) {
      c = /rgba\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]?(?:\.[0-9]+)?)\s*\)/.exec(a);
      if (c) {
        b = [bG(c[1]), bG(c[2]), bG(c[3]), parseFloat(c[4], 10)]
      } else {
        c = /#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(a);
        if (c) {
          b = [bG(c[1], 16), bG(c[2], 16), bG(c[3], 16), 1]
        }
      }
    }
    var b = [],
      c;
    d(a);
    return {
      get: e,
      brighten: f,
      setOpacity: g
    }
  };
  cu.prototype = {
    init: function (a, c) {
      var d = this;
      d.element = b.createElementNS(t, c);
      d.renderer = a;
      d.attrSetters = {}
    },
    animate: function (a, b, c) {
      var d = bR(b, D, true);
      bC(this);
      if (d) {
        d = bx(d);
        if (c) {
          d.complete = c
        }
        bB(this, a, d)
      } else {
        this.attr(a);
        if (c) {
          c()
        }
      }
    },
    attr: function (c, d) {
      var e = this,
        f, g, h, i, j, k = e.element,
        l = k.nodeName,
        m = e.renderer,
        n, o = e.attrSetters,
        p = e.shadows,
        q = e.htmlNode,
        s, u = e;
      if (bH(c) && bO(d)) {
        f = c;
        c = {};
        c[f] = d
      }
      if (bH(c)) {
        f = c;
        if (l === "circle") {
          f = {
            x: "cx",
            y: "cy"
          }[f] || f
        } else if (f === "strokeWidth") {
          f = "stroke-width"
        }
        u = bP(k, f) || e[f] || 0;
        if (f !== "d" && f !== "visibility") {
          u = parseFloat(u)
        }
      } else {
        for (f in c) {
          n = false;
          g = c[f];
          h = o[f] && o[f](g, f);
          if (h !== false) {
            if (h !== a) {
              g = h
            }
            if (f === "d") {
              if (g && g.join) {
                g = g.join(" ")
              }
              if (/(NaN| {2}|^$)/.test(g)) {
                g = "M 0 0"
              }
              e.d = g
            } else if (f === "x" && l === "text") {
              for (i = 0; i < k.childNodes.length; i++) {
                j = k.childNodes[i];
                if (bP(j, "x") === bP(k, "x")) {
                  bP(j, "x", g)
                }
              }
              if (e.rotation) {
                bP(k, "transform", "rotate(" + e.rotation + " " + g + " " + bG(c.y || bP(k, "y")) + ")")
              }
            } else if (f === "fill") {
              g = m.color(g, k, f)
            } else if (l === "circle" && (f === "x" || f === "y")) {
              f = {
                x: "cx",
                y: "cy"
              }[f] || f
            } else if (l === "rect" && f === "r") {
              bP(k, {
                rx: g,
                ry: g
              });
              n = true
            } else if (f === "translateX" || f === "translateY" || f === "rotation" || f === "verticalAlign") {
              e[f] = g;
              e.updateTransform();
              n = true
            } else if (f === "stroke") {
              g = m.color(g, k, f)
            } else if (f === "dashstyle") {
              f = "stroke-dasharray";
              g = g && g.toLowerCase();
              if (g === "solid") {
                g = N
              } else if (g) {
                g = g.replace("shortdashdotdot", "3,1,1,1,1,1,").replace("shortdashdot", "3,1,1,1").replace("shortdot", "1,1,").replace("shortdash", "3,1,").replace("longdash", "8,3,").replace(/dot/g, "1,3,").replace("dash", "4,3,").replace(/,$/, "").split(",");
                i = g.length;
                while (i--) {
                  g[i] = bG(g[i]) * c["stroke-width"]
                }
                g = g.join(",")
              }
            } else if (f === "isTracker") {
              e[f] = g
            } else if (f === "width") {
              g = bG(g)
            } else if (f === "align") {
              f = "text-anchor";
              g = {
                left: "start",
                center: "middle",
                right: "end"
              }[g]
            } else if (f === "title") {
              var v = b.createElementNS(t, "title");
              v.appendChild(b.createTextNode(g));
              k.appendChild(v)
            }
            if (f === "strokeWidth") {
              f = "stroke-width"
            }
            if (r && f === "stroke-width" && g === 0) {
              g = 1e-6
            }
            if (e.symbolName && /^(x|y|r|start|end|innerR|anchorX|anchorY)/.test(f)) {
              if (!s) {
                e.symbolAttr(c);
                s = true
              }
              n = true
            }
            if (p && /^(width|height|visibility|x|y|d|transform)$/.test(f)) {
              i = p.length;
              while (i--) {
                bP(p[i], f, g)
              }
            }
            if ((f === "width" || f === "height") && l === "rect" && g < 0) {
              g = 0
            }
            if (f === "text") {
              e.textStr = g;
              if (e.added) {
                m.buildText(e)
              }
            } else if (!n) {
              bP(k, f, g)
            }
          }
          if (q && (f === "x" || f === "y" || f === "translateX" || f === "translateY" || f === "visibility")) {
            var w, x = q.length ? q : [this],
              y = x.length,
              z, A;
            for (A = 0; A < y; A++) {
              z = x[A];
              w = z.getBBox();
              q = z.htmlNode;
              bS(q, bE(e.styles, {
                left: w.x + (e.translateX || 0) + M,
                top: w.y + (e.translateY || 0) + M
              }));
              if (f === "visibility") {
                bS(q, {
                  visibility: g
                })
              }
            }
          }
        }
      }
      return u
    },
    symbolAttr: function (a) {
      var b = this;
      bt(["x", "y", "r", "start", "end", "width", "height", "innerR", "anchorX", "anchorY"], function (c) {
        b[c] = bR(a[c], b[c])
      });
      b.attr({
        d: b.renderer.symbols[b.symbolName](b.x, b.y, b.width, b.height, b)
      })
    },
    clip: function (a) {
      return this.attr("clip-path", "url(" + this.renderer.url + "#" + a.id + ")")
    },
    crisp: function (a, b, c, d, g) {
      var h = this,
        i, j = {},
        k = {},
        l;
      a = a || h.strokeWidth || h.attr && h.attr("stroke-width") || 0;
      l = e(a) % 2 / 2;
      k.x = f(b || h.x || 0) + l;
      k.y = f(c || h.y || 0) + l;
      k.width = f((d || h.width || 0) - 2 * l);
      k.height = f((g || h.height || 0) - 2 * l);
      k.strokeWidth = a;
      for (i in k) {
        if (h[i] !== k[i]) {
          h[i] = j[i] = k[i]
        }
      }
      return j
    },
    css: function (a) {
      var b = this,
        c = b.element,
        d = a && a.width && c.nodeName === "text",
        e, f = "",
        g = function (a, b) {
          return "-" + b.toLowerCase()
        };
      if (a && a.color) {
        a.fill = a.color
      }
      a = bE(b.styles, a);
      b.styles = a;
      if (p && !u) {
        if (d) {
          delete a.width
        }
        bS(b.element, a)
      } else {
        for (e in a) {
          f += e.replace(/([A-Z])/g, g) + ":" + a[e] + ";"
        }
        b.attr({
          style: f
        })
      }
      if (d && b.added) {
        b.renderer.buildText(b)
      }
      return b
    },
    on: function (a, b) {
      var c = b;
      if (x && a === "click") {
        a = "touchstart";
        c = function (a) {
          a.preventDefault();
          b()
        }
      }
      this.element["on" + a] = c;
      return this
    },
    translate: function (a, b) {
      return this.attr({
        translateX: a,
        translateY: b
      })
    },
    invert: function () {
      var a = this;
      a.inverted = true;
      a.updateTransform();
      return a
    },
    updateTransform: function () {
      var a = this,
        b = a.translateX || 0,
        c = a.translateY || 0,
        d = a.inverted,
        e = a.rotation,
        f = [];
      if (d) {
        b += a.attr("width");
        c += a.attr("height")
      }
      if (b || c) {
        f.push("translate(" + b + "," + c + ")")
      }
      if (d) {
        f.push("rotate(90) scale(-1,1)")
      } else if (e) {
        f.push("rotate(" + e + " " + a.x + " " + a.y + ")")
      }
      if (f.length) {
        bP(a.element, "transform", f.join(" "))
      }
    },
    toFront: function () {
      var a = this.element;
      a.parentNode.appendChild(a);
      return this
    },
    align: function (a, b, c) {
      var d = this;
      if (!a) {
        a = d.alignOptions;
        b = d.alignByTranslate
      } else {
        d.alignOptions = a;
        d.alignByTranslate = b;
        if (!c) {
          d.renderer.alignedObjects.push(d)
        }
      }
      c = bR(c, d.renderer);
      var f = a.align,
        g = a.verticalAlign,
        h = (c.x || 0) + (a.x || 0),
        i = (c.y || 0) + (a.y || 0),
        j = {};
      if (/^(right|center)$/.test(f)) {
        h += (c.width - (a.width || 0)) / {
          right: 1,
          center: 2
        }[f]
      }
      j[b ? "translateX" : "x"] = e(h);
      if (/^(bottom|middle)$/.test(g)) {
        i += (c.height - (a.height || 0)) / ({
          bottom: 1,
          middle: 2
        }[g] || 1)
      }
      j[b ? "translateY" : "y"] = e(i);
      d[d.placed ? "animate" : "attr"](j);
      d.placed = true;
      d.alignAttr = j;
      return d
    },
    getBBox: function () {
      var a, b, c, d = this.rotation,
        e = d * n;
      try {
        a = bE({}, this.element.getBBox())
      } catch (f) {
        a = {
          width: 0,
          height: 0
        }
      }
      b = a.width;
      c = a.height;
      if (d) {
        a.width = j(c * l(e)) + j(b * k(e));
        a.height = j(c * k(e)) + j(b * l(e))
      }
      return a
    },
    show: function () {
      return this.attr({
        visibility: L
      })
    },
    hide: function () {
      return this.attr({
        visibility: J
      })
    },
    add: function (a) {
      var b = this.renderer,
        c = a || b,
        d = c.element || b.box,
        e = d.childNodes,
        f = this.element,
        g = bP(f, "zIndex"),
        h, i, j, k;
      this.parentInverted = a && a.inverted;
      if (this.textStr !== undefined) {
        b.buildText(this)
      }
      if (a && this.htmlNode) {
        if (!a.htmlNode) {
          a.htmlNode = []
        }
        a.htmlNode.push(this)
      }
      if (g) {
        c.handleZ = true;
        g = bG(g)
      }
      if (c.handleZ) {
        for (j = 0; j < e.length; j++) {
          h = e[j];
          i = bP(h, "zIndex");
          if (h !== f && (bG(i) > g || !bO(g) && bO(i))) {
            d.insertBefore(f, h);
            k = true;
            break
          }
        }
      }
      if (!k) {
        d.appendChild(f)
      }
      this.added = true;
      bA(this, "add");
      return this
    },
    safeRemoveChild: function (a) {
      var b = a.parentNode;
      if (b) {
        b.removeChild(a)
      }
    },
    destroy: function () {
      var a = this,
        b = a.element || {},
        c = a.shadows,
        d = a.box,
        e, f;
      b.onclick = b.onmouseout = b.onmouseover = b.onmousemove = null;
      bC(a);
      if (a.clipPath) {
        a.clipPath = a.clipPath.destroy()
      }
      if (a.stops) {
        for (f = 0; f < a.stops.length; f++) {
          a.stops[f] = a.stops[f].destroy()
        }
        a.stops = null
      }
      a.safeRemoveChild(b);
      if (c) {
        bt(c, function (b) {
          a.safeRemoveChild(b)
        })
      }
      if (d) {
        d.destroy()
      }
      bN(a.renderer.alignedObjects, a);
      for (e in a) {
        delete a[e]
      }
      return null
    },
    empty: function () {
      var a = this.element,
        b = a.childNodes,
        c = b.length;
      while (c--) {
        a.removeChild(b[c])
      }
    },
    shadow: function (a, b) {
      var c = [],
        d, e, f = this.element,
        g = this.parentInverted ? "(-1,-1)" : "(1,1)";
      if (a) {
        for (d = 1; d <= 3; d++) {
          e = f.cloneNode(0);
          bP(e, {
            isShadow: "true",
            stroke: "rgb(0, 0, 0)",
            "stroke-opacity": .05 * d,
            "stroke-width": 7 - 2 * d,
            transform: "translate" + g,
            fill: N
          });
          if (b) {
            b.element.appendChild(e)
          } else {
            f.parentNode.insertBefore(e, f)
          }
          c.push(e)
        }
        this.shadows = c
      }
      return this
    }
  };
  var cv = function () {
    this.init.apply(this, arguments)
  };
  cv.prototype = {
    Element: cu,
    init: function (a, b, c, d) {
      var e = this,
        f = location,
        g;
      g = e.createElement("svg").attr({
        xmlns: t,
        version: "1.1"
      });
      a.appendChild(g.element);
      e.box = g.element;
      e.boxWrapper = g;
      e.alignedObjects = [];
      e.url = p ? "" : f.href.replace(/#.*?$/, "");
      e.defs = this.createElement("defs").add();
      e.forExport = d;
      e.gradients = [];
      e.setSize(b, c, false)
    },
    destroy: function () {
      var a = this,
        b, c = a.gradients,
        d = a.defs;
      a.box = null;
      a.boxWrapper = a.boxWrapper.destroy();
      if (c) {
        for (b = 0; b < c.length; b++) {
          a.gradients[b] = c[b].destroy()
        }
        a.gradients = null
      }
      if (d) {
        a.defs = d.destroy()
      }
      a.alignedObjects = null;
      return null
    },
    createElement: function (a) {
      var b = new this.Element;
      b.init(this, a);
      return b
    },
    buildText: function (a) {
      var d = a.element,
        e = bR(a.textStr, "").toString().replace(/<(b|strong)>/g, '<span style="font-weight:bold">').replace(/<(i|em)>/g, '<span style="font-style:italic">').replace(/<a/g, "<span").replace(/<\/(b|strong|i|em|a)>/g, "</span>").split(/<br.*?>/g),
        f = d.childNodes,
        g = /style="([^"]+)"/,
        h = /href="([^"]+)"/,
        i = bP(d, "x"),
        j = a.styles,
        k = j && a.useHTML && !this.forExport,
        l = a.htmlNode,
        m = j && bG(j.width),
        n = j && j.lineHeight,
        o, p = "getComputedStyle",
        q = f.length;
      while (q--) {
        d.removeChild(f[q])
      }
      if (m && !a.added) {
        this.box.appendChild(d)
      }
      if (e[e.length - 1] === "") {
        e.pop()
      }
      bt(e, function (e, f) {
        var j, k = 0,
          l;
        e = e.replace(/<span/g, "|||<span").replace(/<\/span>/g, "</span>|||");
        j = e.split("|||");
        bt(j, function (e) {
          if (e !== "" || j.length === 1) {
            var q = {},
              r = b.createElementNS(t, "tspan");
            if (g.test(e)) {
              bP(r, "style", e.match(g)[1].replace(/(;| |^)color([ :])/, "$1fill$2"))
            }
            if (h.test(e)) {
              bP(r, "onclick", 'location.href="' + e.match(h)[1] + '"');
              bS(r, {
                cursor: "pointer"
              })
            }
            e = (e.replace(/<(.|\n)*?>/g, "") || " ").replace(/</g, "<").replace(/>/g, ">");
            r.appendChild(b.createTextNode(e));
            if (!k) {
              q.x = i
            } else {
              q.dx = 3
            }
            if (!k) {
              if (f) {
                if (!u && a.renderer.forExport) {
                  bS(r, {
                    display: "block"
                  })
                }
                l = c[p] && bG(c[p](o, null).getPropertyValue("line-height"));
                if (!l || isNaN(l)) {
                  l = n || o.offsetHeight || 18
                }
                bP(r, "dy", l)
              }
              o = r
            }
            bP(r, q);
            d.appendChild(r);
            k++;
            if (m) {
              var s = e.replace(/-/g, "- ").split(" "),
                v, w, x = [];
              while (s.length || x.length) {
                w = a.getBBox().width;
                v = w > m;
                if (!v || s.length === 1) {
                  s = x;
                  x = [];
                  if (s.length) {
                    r = b.createElementNS(t, "tspan");
                    bP(r, {
                      dy: n || 16,
                      x: i
                    });
                    d.appendChild(r);
                    if (w > m) {
                      m = w
                    }
                  }
                } else {
                  r.removeChild(r.firstChild);
                  x.unshift(s.pop())
                }
                if (s.length) {
                  r.appendChild(b.createTextNode(s.join(" ").replace(/- /g, "-")))
                }
              }
            }
          }
        })
      });
      if (k) {
        if (!l) {
          l = a.htmlNode = bT("span", null, bE(j, {
            position: H,
            top: 0,
            left: 0
          }), this.box.parentNode)
        }
        l.innerHTML = a.textStr;
        q = f.length;
        while (q--) {
          f[q].style.visibility = J
        }
      }
    },
    button: function (a, b, c, d, e, f, g) {
      var h = this.label(a, b, c),
        i = 0,
        j, k, l, m, n, o = "style",
        p = {
          x1: 0,
          y1: 0,
          x2: 0,
          y2: 1
        };
      e = bx(bF(be, 1, bd, "#999", ba, bF(bb, p, bc, [
        [0, "#FFF"],
        [1, "#DDD"]
      ]), "r", 3, "padding", 3, o, bF("color", "black")), e);
      l = e[o];
      delete e[o];
      f = bx(e, bF(bd, "#68A", ba, bF(bb, p, bc, [
        [0, "#FFF"],
        [1, "#ACF"]
      ])), f);
      m = f[o];
      delete f[o];
      g = bx(e, bF(bd, "#68A", ba, bF(bb, p, bc, [
        [0, "#9BD"],
        [1, "#CDF"]
      ])), g);
      n = g[o];
      delete g[o];
      by(h.element, "mouseenter", function () {
        h.attr(f).css(m)
      });
      by(h.element, "mouseleave", function () {
        j = [e, f, g][i];
        k = [l, m, n][i];
        h.attr(j).css(k)
      });
      h.setState = function (a) {
        i = a;
        if (!a) {
          h.attr(e).css(l)
        } else if (a === 2) {
          h.attr(g).css(n)
        }
      };
      return h.on("click", function () {
        d.call(h)
      }).attr(e).css(bE({
        cursor: "default"
      }, l))
    },
    crispLine: function (a, b) {
      if (a[1] === a[4]) {
        a[1] = a[4] = e(a[1]) + b % 2 / 2
      }
      if (a[2] === a[5]) {
        a[2] = a[5] = e(a[2]) + b % 2 / 2
      }
      return a
    },
    path: function (a) {
      return this.createElement("path").attr({
        d: a,
        fill: N
      })
    },
    circle: function (a, b, c) {
      var d = bI(a) ? a : {
        x: a,
        y: b,
        r: c
      };
      return this.createElement("circle").attr(d)
    },
    arc: function (a, b, c, d, e, f) {
      if (bI(a)) {
        b = a.y;
        c = a.r;
        d = a.innerR;
        e = a.start;
        f = a.end;
        a = a.x
      }
      return this.symbol("arc", a || 0, b || 0, c || 0, c || 0, {
        innerR: d || 0,
        start: e || 0,
        end: f || 0
      })
    },
    rect: function (a, b, c, d, e, f) {
      if (bI(a)) {
        b = a.y;
        c = a.width;
        d = a.height;
        e = a.r;
        f = a.strokeWidth;
        a = a.x
      }
      var g = this.createElement("rect").attr({
        rx: e,
        ry: e,
        fill: N
      });
      return g.attr(g.crisp(f, a, b, h(c, 0), h(d, 0)))
    },
    setSize: function (a, b, c) {
      var d = this,
        e = d.alignedObjects,
        f = e.length;
      d.width = a;
      d.height = b;
      d.boxWrapper[bR(c, true) ? "animate" : "attr"]({
        width: a,
        height: b
      });
      while (f--) {
        e[f].align()
      }
    },
    g: function (a) {
      var b = this.createElement("g");
      return bO(a) ? b.attr({
        "class": K + a
      }) : b
    },
    image: function (a, b, c, d, e) {
      var f = {
        preserveAspectRatio: N
      },
        g;
      if (arguments.length > 1) {
        bE(f, {
          x: b,
          y: c,
          width: d,
          height: e
        })
      }
      g = this.createElement("image").attr(f);
      if (g.element.setAttributeNS) {
        g.element.setAttributeNS("http://www.w3.org/1999/xlink", "href", a)
      } else {
        g.element.setAttribute("hc-svg-href", a)
      }
      return g
    },
    symbol: function (a, b, c, d, f, g) {
      var h, i = this.symbols[a],
        j = i && i(e(b), e(c), d, f, g),
        k = /^url\((.*?)\)$/,
        l, m;
      if (j) {
        h = this.path(j);
        bE(h, {
          symbolName: a,
          x: b,
          y: c,
          width: d,
          height: f
        });
        if (g) {
          bE(h, g)
        }
      } else if (k.test(a)) {
        var n = function (a, b) {
          a.attr({
            width: b[0],
            height: b[1]
          }).translate(-e(b[0] / 2), -e(b[1] / 2))
        };
        l = a.match(k)[1];
        m = y[l];
        h = this.image(l).attr({
          x: b,
          y: c
        });
        if (m) {
          n(h, m)
        } else {
          h.attr({
            width: 0,
            height: 0
          });
          bT("img", {
            onload: function () {
              var a = this;
              n(h, y[l] = [a.width, a.height])
            },
            src: l
          })
        }
      }
      return h
    },
    symbols: {
      circle: function (a, b, c, d) {
        var e = .166 * c;
        return [O, a + c / 2, b, "C", a + c + e, b, a + c + e, b + d, a + c / 2, b + d, "C", a - e, b + d, a - e, b, a + c / 2, b, "Z"]
      },
      square: function (a, b, c, d) {
        return [O, a, b, P, a + c, b, a + c, b + d, a, b + d, "Z"]
      },
      triangle: function (a, b, c, d) {
        return [O, a + c / 2, b, P, a + c, b + d, a, b + d, "Z"]
      },
      "triangle-down": function (a, b, c, d) {
        return [O, a, b, P, a + c, b, a + c / 2, b + d, "Z"]
      },
      diamond: function (a, b, c, d) {
        return [O, a + c / 2, b, P, a + c, b + d / 2, a + c / 2, b + d, a, b + d / 2, "Z"]
      },
      arc: function (a, b, c, d, e) {
        var f = e.start,
          g = e.r || c || d,
          h = e.end - 1e-6,
          i = e.innerR,
          j = k(f),
          n = l(f),
          o = k(h),
          p = l(h),
          q = e.end - f < m ? 0 : 1;
        return [O, a + g * j, b + g * n, "A", g, g, 0, q, 1, a + g * o, b + g * p, P, a + i * o, b + i * p, "A", i, i, 0, q, 0, a + i * j, b + i * n, "Z"]
      }
    },
    clipRect: function (a, b, c, d) {
      var e, f = K + z++,
        g = this.createElement("clipPath").attr({
          id: f
        }).add(this.defs);
      e = this.rect(a, b, c, d, 0).add(g);
      e.id = f;
      e.clipPath = g;
      return e
    },
    color: function (a, b, c) {
      var d, e = /^rgba/;
      if (a && a.linearGradient) {
        var f = this,
          g = a[bb],
          h = !g.length,
          i = K + z++,
          j, k, l;
        j = f.createElement(bb).attr(bE({
          id: i,
          x1: g.x1 || g[0] || 0,
          y1: g.y1 || g[1] || 0,
          x2: g.x2 || g[2] || 0,
          y2: g.y2 || g[3] || 0
        }, h ? null : {
          gradientUnits: "userSpaceOnUse"
        })).add(f.defs);
        f.gradients.push(j);
        j.stops = [];
        bt(a.stops, function (a) {
          var b;
          if (e.test(a[1])) {
            d = ct(a[1]);
            k = d.get("rgb");
            l = d.get("a")
          } else {
            k = a[1];
            l = 1
          }
          b = f.createElement("stop").attr({
            offset: a[0],
            "stop-color": k,
            "stop-opacity": l
          }).add(j);
          j.stops.push(b)
        });
        return "url(" + this.url + "#" + i + ")"
      } else if (e.test(a)) {
        d = ct(a);
        bP(b, c + "-opacity", d.get("a"));
        return d.get("rgb")
      } else {
        b.removeAttribute(c + "-opacity");
        return a
      }
    },
    text: function (a, b, c, d) {
      var f = this,
        g = B.chart.style,
        h;
      b = e(bR(b, 0));
      c = e(bR(c, 0));
      h = f.createElement("text").attr({
        x: b,
        y: c,
        text: a
      }).css({
        fontFamily: g.fontFamily,
        fontSize: g.fontSize
      });
      h.x = b;
      h.y = c;
      h.useHTML = d;
      return h
    },
    label: function (b, c, d, f, g, h) {
      function z() {
        j.attr({
          text: b,
          x: c,
          y: d,
          anchorX: g,
          anchorY: h
        })
      }

      function y(a, b) {
        if (l) {
          l.attr(a, b)
        } else {
          u[a] = b
        }
      }

      function x() {
        var a = j.styles,
          b = a && a.textAlign,
          c = o,
          d = o + e(bG(j.element.style.fontSize || 11) * 1.2);
        if (bO(p) && (b === "center" || b === "right")) {
          c += {
            center: .5,
            right: 1
          }[b] * (p - m.width)
        }
        if (c !== k.x || d !== k.y) {
          k.attr({
            x: c,
            y: d
          })
        }
        k.x = c;
        k.y = d
      }

      function w() {
        m = (p === undefined || q === undefined || j.styles.textAlign) && k.getBBox(true);
        j.width = (p || m.width) + 2 * o;
        j.height = (q || m.height) + 2 * o;
        if (!l) {
          j.box = l = f ? i.symbol(f, 0, 0, j.width, j.height) : i.rect(0, 0, j.width, j.height, 0, u[be]);
          l.add(j)
        }
        l.attr(bx({
          width: j.width,
          height: j.height
        }, u));
        u = null
      }
      var i = this,
        j = i.g(),
        k = i.text().attr({
          zIndex: 1
        }).add(j),
        l, m, n = "left",
        o = 3,
        p, q, r, s, t = 0,
        u = {},
        v = j.attrSetters;
      by(j, "add", z);
      v.width = function (a) {
        p = a;
        return false
      };
      v.height = function (a) {
        q = a;
        return false
      };
      v.padding = function (a) {
        o = a;
        x();
        return false
      };
      v.align = function (a) {
        n = a;
        return false
      };
      v.text = function (a, b) {
        k.attr(b, a);
        w();
        x();
        return false
      };
      v[be] = function (a, b) {
        t = a % 2 / 2;
        y(b, a);
        return false
      };
      v.stroke = v.fill = v.r = function (a, b) {
        y(b, a);
        return false
      };
      v.anchorX = function (a, b) {
        g = a;
        y(b, a + t - r);
        return false
      };
      v.anchorY = function (a, b) {
        h = a;
        y(b, a - s);
        return false
      };
      v.x = function (a) {
        r = a;
        r -= {
          left: 0,
          center: .5,
          right: 1
        }[n] * ((p || m.width) + o);
        j.attr("translateX", e(r));
        return false
      };
      v.y = function (a) {
        s = a;
        j.attr("translateY", e(a));
        return false
      };
      var A = j.css;
      return bE(j, {
        css: function (b) {
          if (b) {
            var c = {};
            b = bx({}, b);
            bt(["fontSize", "fontWeight", "fontFamily", "color", "lineHeight", "width"], function (d) {
              if (b[d] !== a) {
                c[d] = b[d];
                delete b[d]
              }
            });
            k.css(c)
          }
          return A.call(j, b)
        },
        getBBox: function () {
          return l.getBBox()
        },
        shadow: function (a) {
          l.shadow(a);
          return j
        },
        destroy: function () {
          bz(j, "add", z);
          bz(j.element, "mouseenter");
          bz(j.element, "mouseleave");
          if (k) {
            k = k.destroy()
          }
          cu.prototype.destroy.call(j)
        }
      })
    }
  };
  w = cv;
  var cw;
  if (!u) {
    var cx = bU(cu, {
      init: function (a, b) {
        var c = this,
          d = ["<", b, ' filled="f" stroked="f"'],
          e = ["position: ", H, ";"];
        if (b === "shape" || b === G) {
          e.push("left:0;top:0;width:10px;height:10px;")
        }
        if (q) {
          e.push("visibility: ", b === G ? J : L)
        }
        d.push(' style="', e.join(""), '"/>');
        if (b) {
          d = b === G || b === "span" || b === "img" ? d.join("") : a.prepVML(d);
          c.element = bT(d)
        }
        c.renderer = a;
        c.attrSetters = {}
      },
      add: function (a) {
        var b = this,
          c = b.renderer,
          d = b.element,
          e = c.box,
          f = a && a.inverted,
          g = a ? a.element || a : e;
        if (f) {
          c.invertChild(d, g)
        }
        if (q && g.gVis === J) {
          bS(d, {
            visibility: J
          })
        }
        g.appendChild(d);
        b.added = true;
        if (b.alignOnAdd && !b.deferUpdateTransform) {
          b.updateTransform()
        }
        bA(b, "add");
        return b
      },
      toggleChildren: function (a, b) {
        var c = a.childNodes,
          d = c.length;
        while (d--) {
          bS(c[d], {
            visibility: b
          });
          if (c[d].nodeName === "DIV") {
            this.toggleChildren(c[d], b)
          }
        }
      },
      attr: function (b, c) {
        var d = this,
          f, g, i, j, k = d.element || {},
          l = k.style,
          m = k.nodeName,
          n = d.renderer,
          o = d.symbolName,
          p, r = d.shadows,
          s, t = d.attrSetters,
          u = d;
        if (bH(b) && bO(c)) {
          f = b;
          b = {};
          b[f] = c
        }
        if (bH(b)) {
          f = b;
          if (f === "strokeWidth" || f === "stroke-width") {
            u = d.strokeweight
          } else {
            u = d[f]
          }
        } else {
          for (f in b) {
            g = b[f];
            s = false;
            j = t[f] && t[f](g, f);
            if (j !== false) {
              if (j !== a) {
                g = j
              }
              if (o && /^(x|y|r|start|end|width|height|innerR|anchorX|anchorY)/.test(f)) {
                if (!p) {
                  d.symbolAttr(b);
                  p = true
                }
                s = true
              } else if (f === "d") {
                g = g || [];
                d.d = g.join(" ");
                i = g.length;
                var v = [];
                while (i--) {
                  if (bK(g[i])) {
                    v[i] = e(g[i] * 10) - 5
                  } else if (g[i] === "Z") {
                    v[i] = "x"
                  } else {
                    v[i] = g[i]
                  }
                }
                g = v.join(" ") || "x";
                k.path = g;
                if (r) {
                  i = r.length;
                  while (i--) {
                    r[i].path = g
                  }
                }
                s = true
              } else if (f === "zIndex" || f === "visibility") {
                if (q && f === "visibility" && m === "DIV") {
                  k.gVis = g;
                  d.toggleChildren(k, g);
                  if (g === L) {
                    g = null
                  }
                }
                if (g) {
                  l[f] = g
                }
                s = true
              } else if (f === "width" || f === "height") {
                g = h(0, g);
                this[f] = g;
                if (d.updateClipping) {
                  d[f] = g;
                  d.updateClipping()
                } else {
                  l[f] = g
                }
                s = true
              } else if (/^(x|y)$/.test(f)) {
                d[f] = g;
                if (k.tagName === "SPAN") {
                  d.updateTransform()
                } else {
                  l[{
                    x: "left",
                    y: "top"
                  }[f]] = g
                }
              } else if (f === "class") {
                k.className = g
              } else if (f === "stroke") {
                g = n.color(g, k, f);
                f = "strokecolor"
              } else if (f === "stroke-width" || f === "strokeWidth") {
                k.stroked = g ? true : false;
                f = "strokeweight";
                d[f] = g;
                if (bK(g)) {
                  g += M
                }
              } else if (f === "dashstyle") {
                var w = k.getElementsByTagName("stroke")[0] || bT(n.prepVML(["<stroke/>"]), null, null, k);
                w[f] = g || "solid";
                d.dashstyle = g;
                s = true
              } else if (f === "fill") {
                if (m === "SPAN") {
                  l.color = g
                } else {
                  k.filled = g !== N ? true : false;
                  g = n.color(g, k, f);
                  f = "fillcolor"
                }
              } else if (f === "translateX" || f === "translateY" || f === "rotation" || f === "align") {
                if (f === "align") {
                  f = "textAlign"
                }
                d[f] = g;
                d.updateTransform();
                s = true
              } else if (f === "text") {
                this.bBox = null;
                k.innerHTML = g;
                s = true
              }
              if (r && f === "visibility") {
                i = r.length;
                while (i--) {
                  r[i].style[f] = g
                }
              }
              if (!s) {
                if (q) {
                  k[f] = g
                } else {
                  bP(k, f, g)
                }
              }
            }
          }
        }
        return u
      },
      clip: function (a) {
        var b = this,
          c = a.members;
        c.push(b);
        b.destroyClip = function () {
          bN(c, b)
        };
        return b.css(a.getCSS(b.inverted))
      },
      css: function (a) {
        var b = this,
          c = b.element,
          d = a && c.tagName === "SPAN" && a.width;
        if (d) {
          delete a.width;
          b.textWidth = d;
          b.updateTransform()
        }
        b.styles = bE(b.styles, a);
        bS(b.element, a);
        return b
      },
      safeRemoveChild: function (a) {
        var b = a.parentNode;
        if (b) {
          cj(a)
        }
      },
      destroy: function () {
        var a = this;
        if (a.destroyClip) {
          a.destroyClip()
        }
        return cu.prototype.destroy.apply(a)
      },
      empty: function () {
        var a = this.element,
          b = a.childNodes,
          c = b.length,
          d;
        while (c--) {
          d = b[c];
          d.parentNode.removeChild(d)
        }
      },
      getBBox: function (a) {
        var b = this,
          c = b.element,
          d = b.bBox;
        if (!d || a) {
          if (c.nodeName === "text") {
            c.style.position = H
          }
          d = b.bBox = {
            x: c.offsetLeft,
            y: c.offsetTop,
            width: c.offsetWidth,
            height: c.offsetHeight
          }
        }
        return d
      },
      on: function (a, b) {
        this.element["on" + a] = function () {
          var a = c.event;
          a.target = a.srcElement;
          b(a)
        };
        return this
      },
      updateTransform: function () {
        if (!this.added) {
          this.alignOnAdd = true;
          return
        }
        var a = this,
          b = a.element,
          c = a.translateX || 0,
          d = a.translateY || 0,
          f = a.x || 0,
          g = a.y || 0,
          h = a.textAlign || "left",
          i = {
            left: 0,
            center: .5,
            right: 1
          }[h],
          j = h && h !== "left",
          m = a.shadows;
        if (c || d) {
          bS(b, {
            marginLeft: c,
            marginTop: d
          });
          if (m) {
            bt(m, function (a) {
              bS(a, {
                marginLeft: c + 1,
                marginTop: d + 1
              })
            })
          }
        }
        if (a.inverted) {
          bt(b.childNodes, function (c) {
            a.renderer.invertChild(c, b)
          })
        }
        if (b.tagName === "SPAN") {
          var o, p, q = a.rotation,
            r, s = 0,
            t = 1,
            u = 0,
            v, w = bG(a.textWidth),
            x = a.xCorr || 0,
            y = a.yCorr || 0,
            z = [q, h, b.innerHTML, a.textWidth].join(",");
          if (z !== a.cTT) {
            if (bO(q)) {
              s = q * n;
              t = k(s);
              u = l(s);
              bS(b, {
                filter: q ? ["progid:DXImageTransform.Microsoft.Matrix(M11=", t, ", M12=", -u, ", M21=", u, ", M22=", t, ", sizingMethod='auto expand')"].join("") : N
              })
            }
            o = bR(a.elemWidth, b.offsetWidth);
            p = bR(a.elemHeight, b.offsetHeight);
            if (o > w) {
              bS(b, {
                width: w + M,
                display: "block",
                whiteSpace: "normal"
              });
              o = w
            }
            r = e((bG(b.style.fontSize) || 12) * 1.2);
            x = t < 0 && -o;
            y = u < 0 && -p;
            v = t * u < 0;
            x += u * r * (v ? 1 - i : i);
            y -= t * r * (q ? v ? i : 1 - i : 1);
            if (j) {
              x -= o * i * (t < 0 ? -1 : 1);
              if (q) {
                y -= p * i * (u < 0 ? -1 : 1)
              }
              bS(b, {
                textAlign: h
              })
            }
            a.xCorr = x;
            a.yCorr = y
          }
          bS(b, {
            left: f + x,
            top: g + y
          });
          a.cTT = z
        }
      },
      shadow: function (a, b) {
        var c = [],
          d, e = this.element,
          f = this.renderer,
          g, h = e.style,
          i, j = e.path;
        if (j && typeof j.value !== "string") {
          j = "x"
        }
        if (a) {
          for (d = 1; d <= 3; d++) {
            i = ['<shape isShadow="true" strokeweight="', 7 - 2 * d, '" filled="false" path="', j, '" coordsize="100,100" style="', e.style.cssText, '" />'];
            g = bT(f.prepVML(i), null, {
              left: bG(h.left) + 1,
              top: bG(h.top) + 1
            });
            i = ['<stroke color="black" opacity="', .05 * d, '"/>'];
            bT(f.prepVML(i), null, null, g);
            if (b) {
              b.element.appendChild(g)
            } else {
              e.parentNode.insertBefore(g, e)
            }
            c.push(g)
          }
          this.shadows = c
        }
        return this
      }
    });
    cw = function () {
      this.init.apply(this, arguments)
    };
    cw.prototype = bx(cv.prototype, {
      Element: cx,
      isIE8: o.indexOf("MSIE 8.0") > -1,
      init: function (a, c, d) {
        var e = this,
          f;
        e.alignedObjects = [];
        f = e.createElement(G);
        a.appendChild(f.element);
        e.box = f.element;
        e.boxWrapper = f;
        e.setSize(c, d, false);
        if (!b.namespaces.hcv) {
          b.namespaces.add("hcv", "urn:schemas-microsoft-com:vml");
          b.createStyleSheet().cssText = "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke" + "{ behavior:url(#default#VML); display: inline-block; } "
        }
      },
      clipRect: function (a, b, c, d) {
        var f = this.createElement();
        return bE(f, {
          members: [],
          left: a,
          top: b,
          width: c,
          height: d,
          getCSS: function (a) {
            var b = this,
              c = b.top,
              d = b.left,
              f = d + b.width,
              g = c + b.height,
              h = {
                clip: "rect(" + e(a ? d : c) + "px," + e(a ? g : f) + "px," + e(a ? f : g) + "px," + e(a ? c : d) + "px)"
              };
            if (!a && q) {
              bE(h, {
                width: f + M,
                height: g + M
              })
            }
            return h
          },
          updateClipping: function () {
            bt(f.members, function (a) {
              a.css(f.getCSS(a.inverted))
            })
          }
        })
      },
      color: function (a, b, c) {
        var e, f = /^rgba/,
          g;
        if (a && a[bb]) {
          var h, i, j = a[bb],
            k = j.x1 || j[0] || 0,
            l = j.y1 || j[1] || 0,
            n = j.x2 || j[2] || 0,
            o = j.y2 || j[3] || 0,
            p, q, r, s, t;
          bt(a.stops, function (a, b) {
            if (f.test(a[1])) {
              e = ct(a[1]);
              h = e.get("rgb");
              i = e.get("a")
            } else {
              h = a[1];
              i = 1
            }
            if (!b) {
              q = h;
              r = i
            } else {
              s = h;
              t = i
            }
          });
          p = 90 - d.atan((o - l) / (n - k)) * 180 / m;
          g = ["<", c, ' colors="0% ', q, ",100% ", s, '" angle="', p, '" opacity="', t, '" o:opacity2="', r, '" type="gradient" focus="100%" method="any" />'];
          bT(this.prepVML(g), null, null, b)
        } else if (f.test(a) && b.tagName !== "IMG") {
          e = ct(a);
          g = ["<", c, ' opacity="', e.get("a"), '"/>'];
          bT(this.prepVML(g), null, null, b);
          return e.get("rgb")
        } else {
          var u = b.getElementsByTagName(c);
          if (u.length) {
            u[0].opacity = 1
          }
          return a
        }
      },
      prepVML: function (a) {
        var b = "display:inline-block;behavior:url(#default#VML);",
          c = this.isIE8;
        a = a.join("");
        if (c) {
          a = a.replace("/>", ' xmlns="urn:schemas-microsoft-com:vml" />');
          if (a.indexOf('style="') === -1) {
            a = a.replace("/>", ' style="' + b + '" />')
          } else {
            a = a.replace('style="', 'style="' + b)
          }
        } else {
          a = a.replace("<", "<hcv:")
        }
        return a
      },
      text: function (a, b, c) {
        var d = B.chart.style;
        return this.createElement("span").attr({
          text: a,
          x: e(b),
          y: e(c)
        }).css({
          whiteSpace: "nowrap",
          fontFamily: d.fontFamily,
          fontSize: d.fontSize
        })
      },
      path: function (a) {
        return this.createElement("shape").attr({
          coordsize: "100 100",
          d: a
        })
      },
      circle: function (a, b, c) {
        return this.symbol("circle").attr({
          x: a,
          y: b,
          r: c
        })
      },
      g: function (a) {
        var b, c;
        if (a) {
          c = {
            className: K + a,
            "class": K + a
          }
        }
        b = this.createElement(G).attr(c);
        return b
      },
      image: function (a, b, c, d, e) {
        var f = this.createElement("img").attr({
          src: a
        });
        if (arguments.length > 1) {
          f.css({
            left: b,
            top: c,
            width: d,
            height: e
          })
        }
        return f
      },
      rect: function (a, b, c, d, e, f) {
        if (bI(a)) {
          b = a.y;
          c = a.width;
          d = a.height;
          f = a.strokeWidth;
          a = a.x
        }
        var g = this.symbol("rect");
        g.r = e;
        return g.attr(g.crisp(f, a, b, h(c, 0), h(d, 0)))
      },
      invertChild: function (a, b) {
        var c = b.style;
        bS(a, {
          flip: "x",
          left: bG(c.width) - 10,
          top: bG(c.height) - 10,
          rotation: -90
        })
      },
      symbols: {
        arc: function (a, b, c, d, e) {
          var f = e.start,
            g = e.end,
            h = e.r || c || d,
            i = k(f),
            j = l(f),
            n = k(g),
            o = l(g),
            p = e.innerR,
            q = .07 / h,
            r = p && .1 / p || 0;
          if (g - f === 0) {
            return ["x"]
          } else if (2 * m - g + f < q) {
            n = -q
          } else if (g - f < r) {
            n = k(f + r)
          }
          return ["wa", a - h, b - h, a + h, b + h, a + h * i, b + h * j, a + h * n, b + h * o, "at", a - p, b - p, a + p, b + p, a + p * n, b + p * o, a + p * i, b + p * j, "x", "e"]
        },
        circle: function (a, b, c, d) {
          return ["wa", a, b, a + c, b + d, a + c, b + d / 2, a + c, b + d / 2, "e"]
        },
        rect: function (a, b, c, d, e) {
          if (!bO(e)) {
            return []
          }
          var f = a + c,
            g = b + d,
            h = i(e.r || 0, c, d);
          return [O, a + h, b, P, f - h, b, "wa", f - 2 * h, b, f, b + 2 * h, f - h, b, f, b + h, P, f, g - h, "wa", f - 2 * h, g - 2 * h, f, g, f, g - h, f - h, g, P, a + h, g, "wa", a, g - 2 * h, a + 2 * h, g, a + h, g, a, g - h, P, a, b + h, "wa", a, b, a + 2 * h, b + 2 * h, a, b + h, a + h, b, "x", "e"]
        }
      }
    });
    w = cw
  }
  cy.prototype.callbacks = [];
  var cz = function () { };
  cz.prototype = {
    init: function (a, b, c) {
      var d = this,
        e = a.chart.counters,
        f;
      d.series = a;
      d.applyOptions(b, c);
      d.pointAttr = {};
      if (a.options.colorByPoint) {
        f = a.chart.options.colors;
        if (!d.options) {
          d.options = {}
        }
        d.color = d.options.color = d.color || f[e.color++];
        e.wrapColor(f.length)
      }
      a.chart.pointCount++;
      return d
    },
    applyOptions: function (b, c) {
      var d = this,
        e = d.series,
        f = typeof b;
      d.config = b;
      if (f === "number" || b === null) {
        d.y = b
      } else if (typeof b[0] === "number") {
        d.x = b[0];
        d.y = b[1]
      } else if (f === "object" && typeof b.length !== "number") {
        bE(d, b);
        d.options = b
      } else if (typeof b[0] === "string") {
        d.name = b[0];
        d.y = b[1]
      }
      if (d.x === a) {
        d.x = c === a ? e.autoIncrement() : c
      }
    },
    destroy: function () {
      var a = this,
        b = a.series,
        c = b.chart.hoverPoints,
        d;
      b.chart.pointCount--;
      if (c) {
        a.setState();
        bN(c, a)
      }
      if (a === b.chart.hoverPoint) {
        a.onMouseOut()
      }
      b.chart.hoverPoints = null;
      if (a.graphic || a.dataLabel) {
        bz(a);
        a.destroyElements()
      }
      if (a.legendItem) {
        a.series.chart.legend.destroyItem(a)
      }
      for (d in a) {
        a[d] = null
      }
    },
    destroyElements: function () {
      var a = this,
        b = ["graphic", "tracker", "dataLabel", "group", "connector", "shadowGroup"],
        c, d = 6;
      while (d--) {
        c = b[d];
        if (a[c]) {
          a[c] = a[c].destroy()
        }
      }
    },
    getLabelConfig: function () {
      var a = this;
      return {
        x: a.category,
        y: a.y,
        key: a.name || a.category,
        series: a.series,
        point: a,
        percentage: a.percentage,
        total: a.total || a.stackTotal
      }
    },
    select: function (a, b) {
      var c = this,
        d = c.series,
        e = d.chart;
      a = bR(a, !c.selected);
      c.firePointEvent(a ? "select" : "unselect", {
        accumulate: b
      }, function () {
        c.selected = a;
        c.setState(a && T);
        if (!b) {
          bt(e.getSelectedPoints(), function (a) {
            if (a.selected && a !== c) {
              a.selected = false;
              a.setState(R);
              a.firePointEvent("unselect")
            }
          })
        }
      })
    },
    onMouseOver: function () {
      var a = this,
        b = a.series,
        c = b.chart,
        d = c.tooltip,
        e = c.hoverPoint;
      if (e && e !== a) {
        e.onMouseOut()
      }
      a.firePointEvent("mouseOver");
      if (d && (!d.shared || b.noSharedTooltip)) {
        d.refresh(a)
      }
      a.setState(S);
      c.hoverPoint = a
    },
    onMouseOut: function () {
      var a = this;
      a.firePointEvent("mouseOut");
      a.setState();
      a.series.chart.hoverPoint = null
    },
    tooltipFormatter: function (a) {
      var b = this,
        c = b.series,
        d = c.tooltipOptions,
        e = String(b.y).split("."),
        f = e[1] ? e[1].length : 0,
        g = a.match(/\{(series|point)\.[a-zA-Z]+\}/g),
        h = /[\.}]/,
        i, j, k, l;
      for (l in g) {
        j = g[l];
        if (bH(j) && j !== a) {
          i = j.indexOf("point") === 1 ? b : c;
          if (j === "{point.y}") {
            k = (d.yPrefix || "") + bV(b.y, bR(d.yDecimals, f)) + (d.ySuffix || "")
          } else {
            k = i[g[l].split(h)[1]]
          }
          a = a.replace(g[l], k)
        }
      }
      return a
    },
    update: function (a, b, c) {
      var d = this,
        e = d.series,
        f = d.graphic,
        g, h = e.data,
        i = h.length,
        j = e.chart;
      b = bR(b, true);
      d.firePointEvent("update", {
        options: a
      }, function () {
        d.applyOptions(a);
        if (bI(a)) {
          e.getAttribs();
          if (f) {
            f.attr(d.pointAttr[e.state])
          }
        }
        for (g = 0; g < i; g++) {
          if (h[g] === d) {
            e.xData[g] = d.x;
            e.yData[g] = d.y;
            e.options.data[g] = a;
            break
          }
        }
        e.isDirty = true;
        e.isDirtyData = true;
        if (b) {
          j.redraw(c)
        }
      })
    },
    remove: function (a, b) {
      var c = this,
        d = c.series,
        e = d.chart,
        f, g = d.data,
        h = g.length;
      cc(b, e);
      a = bR(a, true);
      c.firePointEvent("remove", null, function () {
        for (f = 0; f < h; f++) {
          if (g[f] === c) {
            g.splice(f, 1);
            d.options.data.splice(f, 1);
            d.xData.splice(f, 1);
            d.yData.splice(f, 1);
            break
          }
        }
        c.destroy();
        d.isDirty = true;
        d.isDirtyData = true;
        if (a) {
          e.redraw()
        }
      })
    },
    firePointEvent: function (a, b, c) {
      var d = this,
        e = this.series,
        f = e.options;
      if (f.point.events[a] || d.options && d.options.events && d.options.events[a]) {
        this.importEvents()
      }
      if (a === "click" && f.allowPointSelect) {
        c = function (a) {
          d.select(null, a.ctrlKey || a.metaKey || a.shiftKey)
        }
      }
      bA(this, a, b, c)
    },
    importEvents: function () {
      if (!this.hasImportedEvents) {
        var a = this,
          b = bx(a.series.options.point, a.options),
          c = b.events,
          d;
        a.events = c;
        for (d in c) {
          by(a, d, c[d])
        }
        this.hasImportedEvents = true
      }
    },
    setState: function (a) {
      var b = this,
        c = b.plotX,
        d = b.plotY,
        e = b.series,
        f = e.options.states,
        g = cr[e.type].marker && e.options.marker,
        h = g && !g.enabled,
        i = g && g.states[a],
        j = i && i.enabled === false,
        k = e.stateMarkerGraphic,
        l = e.chart,
        m, n = b.pointAttr;
      a = a || R;
      if (a === b.state || b.selected && a !== T || f[a] && f[a].enabled === false || a && (j || h && !i.enabled)) {
        return
      }
      if (b.graphic) {
        m = b.graphic.symbolName && n[a].r;
        b.graphic.attr(bx(n[a], m ? {
          x: c - m,
          y: d - m,
          width: 2 * m,
          height: 2 * m
        } : {}))
      } else {
        if (a) {
          if (!k) {
            m = g.radius;
            e.stateMarkerGraphic = k = l.renderer.symbol(e.symbol, -m, -m, 2 * m, 2 * m).attr(n[a]).add(e.group)
          }
          k.translate(c, d)
        }
        if (k) {
          k[a ? "show" : "hide"]()
        }
      }
      b.state = a
    }
  };
  var cA = function () { };
  cA.prototype = {
    isCartesian: true,
    type: "line",
    pointClass: cz,
    pointAttrToOptions: {
      stroke: "lineColor",
      "stroke-width": "lineWidth",
      fill: "fillColor",
      r: "radius"
    },
    init: function (a, b) {
      var c = this,
        d, e, f = a.series.length;
      c.chart = a;
      c.options = b = c.setOptions(b);
      c.bindAxes();
      bE(c, {
        index: f,
        name: b.name || "Series " + (f + 1),
        state: R,
        pointAttr: {},
        visible: b.visible !== false,
        selected: b.selected === true
      });
      e = b.events;
      for (d in e) {
        by(c, d, e[d])
      }
      if (e && e.click || b.point && b.point.events && b.point.events.click || b.allowPointSelect) {
        a.runTrackerClick = true
      }
      c.getColor();
      c.getSymbol();
      c.setData(b.data, false)
    },
    bindAxes: function () {
      var b = this,
        c = b.options,
        d = b.chart,
        e;
      if (b.isCartesian) {
        bt(["xAxis", "yAxis"], function (f) {
          bt(d[f], function (d) {
            e = d.options;
            if (c[f] === e.index || c[f] === a && e.index === 0) {
              d.series.push(b);
              b[f] = d;
              d.isDirty = true
            }
          })
        })
      }
    },
    autoIncrement: function () {
      var a = this,
        b = a.options,
        c = a.xIncrement;
      c = bR(c, b.pointStart, 0);
      a.pointInterval = bR(a.pointInterval, b.pointInterval, 1);
      a.xIncrement = c + a.pointInterval;
      return c
    },
    getSegments: function () {
      var a = this,
        b = -1,
        c = [],
        d, e = a.points,
        f = e.length;
      if (f) {
        if (a.options.connectNulls) {
          d = f;
          while (d--) {
            if (e[d].y === null) {
              e.splice(d, 1)
            }
          }
          c = [e]
        } else {
          bt(e, function (a, d) {
            if (a.y === null) {
              if (d > b + 1) {
                c.push(e.slice(b + 1, d))
              }
              b = d
            } else if (d === f - 1) {
              c.push(e.slice(b + 1, d + 1))
            }
          })
        }
      }
      a.segments = c
    },
    setOptions: function (a) {
      var b = this,
        c = b.chart,
        d = c.options,
        e = d.plotOptions,
        f = a.data,
        g;
      a.data = null;
      g = bx(e[this.type], e.series, a);
      g.data = f;
      b.tooltipOptions = bx(d.tooltip, g.tooltip);
      return g
    },
    getColor: function () {
      var a = this.chart.options.colors,
        b = this.chart.counters;
      this.color = this.options.color || a[b.color++] || "#0000ff";
      b.wrapColor(a.length)
    },
    getSymbol: function () {
      var a = this,
        b = a.options.marker,
        c = a.chart,
        d = c.options.symbols,
        e = c.counters;
      a.symbol = b.symbol || d[e.symbol++];
      if (/^url/.test(a.symbol)) {
        b.radius = 0
      }
      e.wrapSymbol(d.length)
    },
    addPoint: function (a, b, c, d) {
      var e = this,
        f = e.data,
        g = e.graph,
        h = e.area,
        i = e.chart,
        j = e.xData,
        k = e.yData,
        l = g && g.shift || 0,
        m = e.options.data,
        n;
      cc(d, i);
      if (g && c) {
        g.shift = l + 1
      }
      if (h) {
        h.shift = l + 1;
        h.isArea = true
      }
      b = bR(b, true);
      n = {
        series: e
      };
      e.pointClass.prototype.applyOptions.apply(n, [a]);
      j.push(n.x);
      k.push(e.valueCount === 4 ? [n.open, n.high, n.low, n.close] : n.y);
      m.push(a);
      if (c) {
        if (f[0]) {
          f[0].remove(false)
        } else {
          f.shift();
          j.shift();
          k.shift();
          m.shift()
        }
      }
      e.getAttribs();
      e.isDirty = true;
      e.isDirtyData = true;
      if (b) {
        i.redraw()
      }
    },
    setData: function (a, b) {
      var c = this,
        d = c.points,
        e = c.options,
        f = c.initialColor,
        g = c.chart,
        h = null,
        i;
      c.xIncrement = null;
      c.pointRange = c.xAxis && c.xAxis.categories && 1 || e.pointRange;
      if (bO(f)) {
        g.counters.color = f
      }
      var j = [],
        k = [],
        l = a ? a.length : [],
        m = e.turboThreshold || 1e3,
        n, o = c.valueCount === 4;
      if (l > m) {
        i = 0;
        while (h === null && i < l) {
          h = a[i];
          i++
        }
        if (bK(h)) {
          var p = bR(e.pointStart, 0),
            q = bR(e.pointInterval, 1);
          for (i = 0; i < l; i++) {
            j[i] = p;
            k[i] = a[i];
            p += q
          }
          c.xIncrement = p
        } else if (bJ(h)) {
          if (o) {
            for (i = 0; i < l; i++) {
              n = a[i];
              j[i] = n[0];
              k[i] = n.slice(1, 5)
            }
          } else {
            for (i = 0; i < l; i++) {
              n = a[i];
              j[i] = n[0];
              k[i] = n[1]
            }
          }
        }
      } else {
        for (i = 0; i < l; i++) {
          n = {
            series: c
          };
          c.pointClass.prototype.applyOptions.apply(n, [a[i]]);
          j[i] = n.x;
          k[i] = o ? [n.open, n.high, n.low, n.close] : n.y
        }
      }
      c.data = [];
      c.options.data = a;
      c.xData = j;
      c.yData = k;
      i = d && d.length || 0;
      while (i--) {
        if (d[i] && d[i].destroy) {
          d[i].destroy()
        }
      }
      c.isDirty = c.isDirtyData = g.isDirtyBox = true;
      if (bR(b, true)) {
        g.redraw(false)
      }
    },
    remove: function (a, b) {
      var c = this,
        d = c.chart;
      a = bR(a, true);
      if (!c.isRemoving) {
        c.isRemoving = true;
        bA(c, "remove", null, function () {
          c.destroy();
          d.isDirtyLegend = d.isDirtyBox = true;
          if (a) {
            d.redraw(b)
          }
        })
      }
      c.isRemoving = false
    },
    processData: function () {
      var b = this,
        c = b.xData,
        d = b.yData,
        e = c.length,
        f = 0,
        g = e,
        i, j, k, l, m = b.options,
        n = m.cropThreshold;
      if (b.isCartesian && !b.isDirty && !b.xAxis.isDirty && !b.yAxis.isDirty) {
        return false
      }
      if (!n || e > n || b.forceCrop) {
        var o = b.xAxis.getExtremes(),
          p = o.min,
          q = o.max;
        if (c[e - 1] < p || c[0] > q) {
          c = [];
          d = []
        } else if (c[0] < p || c[e - 1] > q) {
          for (l = 0; l < e; l++) {
            if (c[l] >= p) {
              f = h(0, l - 1);
              break
            }
          }
          for (; l < e; l++) {
            if (c[l] > q) {
              g = l + 1;
              break
            }
          }
          c = c.slice(f, g);
          d = d.slice(f, g);
          i = true
        }
      }
      for (l = c.length - 1; l > 0; l--) {
        j = c[l] - c[l - 1];
        if (k === a || j < k) {
          k = j
        }
      }
      b.cropped = i;
      b.cropStart = f;
      b.processedXData = c;
      b.processedYData = d;
      if (m.pointRange === null) {
        b.pointRange = k || 1
      }
      b.closestPointRange = k
    },
    generatePoints: function () {
      var a = this,
        b = a.options,
        c = b.data,
        d = a.data,
        e, f = a.processedXData,
        g = a.processedYData,
        h = a.pointClass,
        i = f.length,
        j = a.cropStart || 0,
        k, l = a.hasGroupedData,
        m, n = [],
        o;
      if (!d && !l) {
        var p = [];
        p.length = c.length;
        d = a.data = p
      }
      for (o = 0; o < i; o++) {
        k = j + o;
        if (!l) {
          if (d[k]) {
            m = d[k]
          } else {
            d[k] = m = (new h).init(a, c[k], f[o])
          }
          n[o] = m
        } else {
          n[o] = (new h).init(a, [f[o]].concat(bQ(g[o])))
        }
      }
      if (d && (i !== (e = d.length) || l)) {
        for (o = 0; o < e; o++) {
          if (o === j && !l) {
            o += i
          }
          if (d[o]) {
            d[o].destroyElements()
          }
        }
      }
      a.data = d;
      a.points = n
    },
    translate: function () {
      if (!this.processedXData) {
        this.processData()
      }
      this.generatePoints();
      var b = this,
        c = b.chart,
        d = b.options,
        f = d.stacking,
        g = b.xAxis,
        h = g.categories,
        i = b.yAxis,
        j = b.points,
        k = j.length,
        l = !!b.modifyValue,
        m = b.index === i.series.length - 1,
        n;
      for (n = 0; n < k; n++) {
        var o = j[n],
          p = o.x,
          q = o.y,
          r = o.low,
          s = i.stacks[(q < d.threshold ? "-" : "") + b.stackKey],
          t, u;
        o.plotX = e(g.translate(p) * 10) / 10;
        if (f && b.visible && s && s[p]) {
          t = s[p];
          u = t.total;
          t.cum = r = t.cum - q;
          q = r + q;
          if (m) {
            r = d.threshold
          }
          if (f === "percent") {
            r = u ? r * 100 / u : 0;
            q = u ? q * 100 / u : 0
          }
          o.percentage = u ? o.y * 100 / u : 0;
          o.stackTotal = u
        }
        if (bO(r)) {
          o.yBottom = i.translate(r, 0, 1, 0, 1)
        }
        if (l) {
          q = b.modifyValue(q, o)
        }
        if (q !== null) {
          o.plotY = e(i.translate(q, 0, 1, 0, 1) * 10) / 10
        }
        o.clientX = c.inverted ? c.plotHeight - o.plotX : o.plotX;
        o.category = h && h[o.x] !== a ? h[o.x] : o.x
      }
      b.getSegments()
    },
    setTooltipPoints: function (a) {
      var b = this,
        c = b.chart,
        d = c.inverted,
        g = [],
        h, i = e((d ? c.plotTop : c.plotLeft) + c.plotSizeX),
        j, k, l = b.xAxis,
        m, n, o = [];
      if (b.options.enableMouseTracking === false) {
        return
      }
      if (a) {
        b.tooltipPoints = null
      }
      bt(b.segments || b.points, function (a) {
        g = g.concat(a)
      });
      if (l && l.reversed) {
        g = g.reverse()
      }
      h = g.length;
      for (n = 0; n < h; n++) {
        m = g[n];
        j = g[n - 1] ? g[n - 1]._high + 1 : 0;
        k = m._high = g[n + 1] ? f((m.plotX + (g[n + 1] ? g[n + 1].plotX : i)) / 2) : i;
        while (j <= k) {
          o[d ? i - j++ : j++] = m
        }
      }
      b.tooltipPoints = o
    },
    tooltipHeaderFormatter: function (a) {
      var b = this,
        c = b.tooltipOptions,
        d = c.xDateFormat || "%A, %b %e, %Y",
        e = b.xAxis,
        f = e && e.options.type === "datetime";
      return c.headerFormat.replace("{point.key}", f ? C(d, a) : a).replace("{series.name}", b.name).replace("{series.color}", b.color)
    },
    onMouseOver: function () {
      var a = this,
        b = a.chart,
        c = b.hoverSeries;
      if (!x && b.mouseIsDown) {
        return
      }
      if (c && c !== a) {
        c.onMouseOut()
      }
      if (a.options.events.mouseOver) {
        bA(a, "mouseOver")
      }
      a.setState(S);
      b.hoverSeries = a
    },
    onMouseOut: function () {
      var a = this,
        b = a.options,
        c = a.chart,
        d = c.tooltip,
        e = c.hoverPoint;
      if (e) {
        e.onMouseOut()
      }
      if (a && b.events.mouseOut) {
        bA(a, "mouseOut")
      }
      if (d && !b.stickyTracking && !d.shared) {
        d.hide()
      }
      a.setState();
      c.hoverSeries = null
    },
    animate: function (a) {
      var b = this,
        c = b.chart,
        d = b.clipRect,
        e = b.options.animation;
      if (e && !bI(e)) {
        e = {}
      }
      if (a) {
        if (!d.isAnimating) {
          d.attr("width", 0);
          d.isAnimating = true
        }
      } else {
        d.animate({
          width: c.plotSizeX
        }, e);
        this.animate = null
      }
    },
    drawPoints: function () {
      var b = this,
        c, d = b.points,
        e = b.chart,
        f, g, h, i, j, k;
      if (b.options.marker.enabled) {
        h = d.length;
        while (h--) {
          i = d[h];
          f = i.plotX;
          g = i.plotY;
          k = i.graphic;
          if (g !== a && !isNaN(g)) {
            c = i.pointAttr[i.selected ? T : R];
            j = c.r;
            if (k) {
              k.animate(bE({
                x: f - j,
                y: g - j
              }, k.symbolName ? {
                width: 2 * j,
                height: 2 * j
              } : {}))
            } else if (j > 0) {
              i.graphic = e.renderer.symbol(bR(i.marker && i.marker.symbol, b.symbol), f - j, g - j, 2 * j, 2 * j).attr(c).add(b.group)
            }
          }
        }
      }
    },
    convertAttribs: function (a, b, c, d) {
      var e = this.pointAttrToOptions,
        f, g, h = {};
      a = a || {};
      b = b || {};
      c = c || {};
      d = d || {};
      for (f in e) {
        g = e[f];
        h[f] = bR(a[g], b[f], c[f], d[f])
      }
      return h
    },
    getAttribs: function () {
      var a = this,
        b = cr[a.type].marker ? a.options.marker : a.options,
        c = b.states,
        d = c[S],
        e, f = a.color,
        g = {
          stroke: f,
          fill: f
        },
        h = a.points,
        i, j, k = [],
        l, m = a.pointAttrToOptions,
        n, o;
      if (a.options.marker) {
        d.radius = d.radius || b.radius + 2;
        d.lineWidth = d.lineWidth || b.lineWidth + 1
      } else {
        d.color = d.color || ct(d.color || f).brighten(d.brightness).get()
      }
      k[R] = a.convertAttribs(b, g);
      bt([S, T], function (b) {
        k[b] = a.convertAttribs(c[b], k[R])
      });
      a.pointAttr = k;
      i = h.length;
      while (i--) {
        j = h[i];
        b = j.options && j.options.marker || j.options;
        if (b && b.enabled === false) {
          b.radius = 0
        }
        n = false;
        if (j.options) {
          for (o in m) {
            if (bO(b[m[o]])) {
              n = true
            }
          }
        }
        if (n) {
          l = [];
          c = b.states || {};
          e = c[S] = c[S] || {};
          if (!a.options.marker) {
            e.color = ct(e.color || j.options.color).brighten(e.brightness || d.brightness).get()
          }
          l[R] = a.convertAttribs(b, k[R]);
          l[S] = a.convertAttribs(c[S], k[S], l[R]);
          l[T] = a.convertAttribs(c[T], k[T], l[R])
        } else {
          l = k
        }
        j.pointAttr = l
      }
    },
    destroy: function () {
      var a = this,
        b = a.chart,
        c = a.clipRect,
        d = /AppleWebKit\/533/.test(o),
        e, f, g = a.data || [],
        h, i, j;
      bA(a, "destroy");
      bz(a);
      bt(["xAxis", "yAxis"], function (b) {
        j = a[b];
        if (j) {
          bN(j.series, a);
          j.isDirty = true
        }
      });
      if (a.legendItem) {
        a.chart.legend.destroyItem(a)
      }
      f = g.length;
      while (f--) {
        h = g[f];
        if (h && h.destroy) {
          h.destroy()
        }
      }
      a.points = null;
      if (c && c !== b.clipRect) {
        a.clipRect = c.destroy()
      }
      bt(["area", "graph", "dataLabelsGroup", "group", "tracker"], function (b) {
        if (a[b]) {
          e = d && b === "group" ? "hide" : "destroy";
          a[b][e]()
        }
      });
      if (b.hoverSeries === a) {
        b.hoverSeries = null
      }
      bN(b.series, a);
      for (i in a) {
        delete a[i]
      }
    },
    drawDataLabels: function () {
      if (this.options.dataLabels.enabled) {
        var a = this,
          b, c, d = a.points,
          e = a.options,
          f = e.dataLabels,
          g, h = a.dataLabelsGroup,
          i = a.chart,
          j = a.xAxis,
          k = j ? j.left : i.plotLeft,
          l = a.yAxis,
          m = l ? l.top : i.plotTop,
          n = i.renderer,
          o = i.inverted,
          p = a.type,
          q, r = e.stacking,
          s = p === "column" || p === "bar",
          t = f.verticalAlign === null,
          u = f.y === null;
        if (s) {
          if (r) {
            if (t) {
              f = bx(f, {
                verticalAlign: "middle"
              })
            }
            if (u) {
              f = bx(f, {
                y: {
                  top: 14,
                  middle: 4,
                  bottom: -6
                }[f.verticalAlign]
              })
            }
          } else {
            if (t) {
              f = bx(f, {
                verticalAlign: "top"
              })
            }
          }
        }
        if (!h) {
          h = a.dataLabelsGroup = n.g("data-labels").attr({
            visibility: a.visible ? L : J,
            zIndex: 6
          }).translate(k, m).add()
        } else {
          h.translate(k, m)
        }
        q = f.color;
        if (q === "auto") {
          q = null
        }
        f.style.color = bR(q, a.color, "black");
        bt(d, function (a) {
          var d = a.barX,
            j = d && d + a.barW / 2 || a.plotX || -999,
            k = bR(a.plotY, -999),
            l = a.dataLabel,
            m = f.align,
            q = u ? a.y >= 0 ? -6 : 12 : f.y;
          g = f.formatter.call(a.getLabelConfig());
          b = (o ? i.plotWidth - k : j) + f.x;
          c = (o ? i.plotHeight - j : k) + q;
          if (p === "column") {
            b += {
              left: -1,
              right: 1
            }[m] * a.barW / 2 || 0
          }
          if (o && a.y < 0) {
            m = "right";
            b -= 10
          }
          if (l) {
            if (o && !f.y) {
              c = c + bG(l.styles.lineHeight) * .9 - l.getBBox().height / 2
            }
            l.attr({
              text: g
            }).animate({
              x: b,
              y: c
            })
          } else if (bO(g)) {
            l = a.dataLabel = n.text(g, b, c).attr({
              align: m,
              rotation: f.rotation,
              zIndex: 1
            }).css(f.style).add(h);
            if (o && !f.y) {
              l.attr({
                y: c + bG(l.styles.lineHeight) * .9 - l.getBBox().height / 2
              })
            }
          }
          if (s && e.stacking && l) {
            var r = a.barY,
              t = a.barW,
              v = a.barH;
            l.align(f, null, {
              x: o ? i.plotWidth - r - v : d,
              y: o ? i.plotHeight - d - t : r,
              width: o ? v : t,
              height: o ? t : v
            })
          }
        })
      }
    },
    drawGraph: function () {
      var a = this,
        b = a.options,
        c = a.chart,
        d = a.graph,
        e = [],
        f, g = a.area,
        h = a.group,
        i = b.lineColor || a.color,
        j = b.lineWidth,
        k = b.dashStyle,
        l, m = c.renderer,
        n = a.yAxis.getThreshold(b.threshold),
        o = /^area/.test(a.type),
        p = [],
        q = [],
        r;
      bt(a.segments, function (c) {
        l = [];
        bt(c, function (d, e) {
          if (a.getPointSpline) {
            l.push.apply(l, a.getPointSpline(c, d, e))
          } else {
            l.push(e ? P : O);
            if (e && b.step) {
              var f = c[e - 1];
              l.push(d.plotX, f.plotY)
            }
            l.push(d.plotX, d.plotY)
          }
        });
        if (c.length > 1) {
          e = e.concat(l)
        } else {
          p.push(c[0])
        }
        if (o) {
          var d = [],
            f, g = l.length;
          for (f = 0; f < g; f++) {
            d.push(l[f])
          }
          if (g === 3) {
            d.push(P, l[1], l[2])
          }
          if (b.stacking && a.type !== "areaspline") {
            for (f = c.length - 1; f >= 0; f--) {
              if (f < c.length - 1 && b.step) {
                d.push(c[f + 1].plotX, c[f].yBottom)
              }
              d.push(c[f].plotX, c[f].yBottom)
            }
          } else {
            d.push(P, c[c.length - 1].plotX, n, P, c[0].plotX, n)
          }
          q = q.concat(d)
        }
      });
      a.graphPath = e;
      a.singlePoints = p;
      if (o) {
        f = bR(b.fillColor, ct(a.color).setOpacity(b.fillOpacity || .75).get());
        if (g) {
          g.animate({
            d: q
          })
        } else {
          a.area = a.chart.renderer.path(q).attr({
            fill: f
          }).add(h)
        }
      }
      if (d) {
        bC(d);
        d.animate({
          d: e
        })
      } else {
        if (j) {
          r = {
            stroke: i,
            "stroke-width": j
          };
          if (k) {
            r.dashstyle = k
          }
          a.graph = m.path(e).attr(r).add(h).shadow(b.shadow)
        }
      }
    },
    render: function () {
      var a = this,
        b = a.chart,
        c, d, e = a.options,
        f = e.clip !== false,
        g = e.animation,
        h = g && a.animate,
        i = h ? g && g.duration || 500 : 0,
        j = a.clipRect,
        k = b.renderer;
      if (!j) {
        j = a.clipRect = !b.hasRendered && b.clipRect ? b.clipRect : k.clipRect(0, 0, b.plotSizeX, b.plotSizeY + 1);
        if (!b.clipRect) {
          b.clipRect = j
        }
      }
      if (!a.group) {
        c = a.group = k.g("series");
        if (b.inverted) {
          d = function () {
            c.attr({
              width: b.plotWidth,
              height: b.plotHeight
            }).invert()
          };
          d();
          by(b, "resize", d);
          by(a, "destroy", function () {
            bz(b, "resize", d)
          })
        }
        if (f) {
          c.clip(a.clipRect)
        }
        c.attr({
          visibility: a.visible ? L : J,
          zIndex: e.zIndex
        }).translate(a.xAxis.left, a.yAxis.top).add(b.seriesGroup)
      }
      a.drawDataLabels();
      if (h) {
        a.animate(true)
      }
      a.getAttribs();
      if (a.drawGraph) {
        a.drawGraph()
      }
      a.drawPoints();
      if (a.options.enableMouseTracking !== false) {
        a.drawTracker()
      }
      if (h) {
        a.animate()
      }
      setTimeout(function () {
        j.isAnimating = false;
        c = a.group;
        if (c && j !== b.clipRect && j.renderer) {
          if (f) {
            c.clip(a.clipRect = b.clipRect)
          }
          j.destroy()
        }
      }, i);
      a.isDirty = a.isDirtyData = false
    },
    redraw: function () {
      var a = this,
        b = a.chart,
        c = a.isDirtyData,
        d = a.group;
      if (d) {
        if (b.inverted) {
          d.attr({
            width: b.plotWidth,
            height: b.plotHeight
          })
        }
        d.animate({
          translateX: a.xAxis.left,
          translateY: a.yAxis.top
        })
      }
      a.translate();
      a.setTooltipPoints(true);
      a.render();
      if (c) {
        bA(a, "updatedData")
      }
    },
    setState: function (a) {
      var b = this,
        c = b.options,
        d = b.graph,
        e = c.states,
        f = c.lineWidth;
      a = a || R;
      if (b.state !== a) {
        b.state = a;
        if (e[a] && e[a].enabled === false) {
          return
        }
        if (a) {
          f = e[a].lineWidth || f + 1
        }
        if (d && !d.dashstyle) {
          d.attr({
            "stroke-width": f
          }, a ? 0 : 500)
        }
      }
    },
    setVisible: function (b, c) {
      var d = this,
        e = d.chart,
        f = d.legendItem,
        g = d.group,
        h = d.tracker,
        i = d.dataLabelsGroup,
        j, k, l = d.points,
        m, n = e.options.chart.ignoreHiddenSeries,
        o = d.visible;
      d.visible = b = b === a ? !o : b;
      j = b ? "show" : "hide";
      if (g) {
        g[j]()
      }
      if (h) {
        h[j]()
      } else if (l) {
        k = l.length;
        while (k--) {
          m = l[k];
          if (m.tracker) {
            m.tracker[j]()
          }
        }
      }
      if (i) {
        i[j]()
      }
      if (f) {
        e.legend.colorizeItem(d, b)
      }
      d.isDirty = true;
      if (d.options.stacking) {
        bt(e.series, function (a) {
          if (a.options.stacking && a.visible) {
            a.isDirty = true
          }
        })
      }
      if (n) {
        e.isDirtyBox = true
      }
      if (c !== false) {
        e.redraw()
      }
      bA(d, j)
    },
    show: function () {
      this.setVisible(true)
    },
    hide: function () {
      this.setVisible(false)
    },
    select: function (b) {
      var c = this;
      c.selected = b = b === a ? !c.selected : b;
      if (c.checkbox) {
        c.checkbox.checked = b
      }
      bA(c, b ? "select" : "unselect")
    },
    drawTracker: function () {
      var a = this,
        b = a.options,
        c = [].concat(a.graphPath),
        d = c.length,
        e = a.chart,
        f = e.options.tooltip.snap,
        g = a.tracker,
        h = b.cursor,
        i = h && {
          cursor: h
        },
        j = a.singlePoints,
        k, l;
      if (d) {
        l = d + 1;
        while (l--) {
          if (c[l] === O) {
            c.splice(l + 1, 0, c[l + 1] - f, c[l + 2], P)
          }
          if (l && c[l] === O || l === d) {
            c.splice(l, 0, P, c[l - 2] + f, c[l - 1])
          }
        }
      }
      for (l = 0; l < j.length; l++) {
        k = j[l];
        c.push(O, k.plotX - f, k.plotY, P, k.plotX + f, k.plotY)
      }
      if (g) {
        g.attr({
          d: c
        })
      } else {
        a.tracker = e.renderer.path(c).attr({
          isTracker: true,
          stroke: Q,
          fill: N,
          "stroke-width": b.lineWidth + 2 * f,
          visibility: a.visible ? L : J,
          zIndex: b.zIndex || 1
        }).on(x ? "touchstart" : "mouseover", function () {
          if (e.hoverSeries !== a) {
            a.onMouseOver()
          }
        }).on("mouseout", function () {
          if (!b.stickyTracking) {
            a.onMouseOut()
          }
        }).css(i).add(e.trackerGroup)
      }
    }
  };
  var cB = bU(cA);
  bD.line = cB;
  var cC = bU(cA, {
    type: "area",
    useThreshold: true
  });
  bD.area = cC;
  var cD = bU(cA, {
    type: "spline",
    getPointSpline: function (a, b, c) {
      var d = 1.5,
        e = d + 1,
        f = b.plotX,
        g = b.plotY,
        j = a[c - 1],
        k = a[c + 1],
        l, m, n, o, p;
      if (c && c < a.length - 1) {
        var q = j.plotX,
          r = j.plotY,
          s = k.plotX,
          t = k.plotY,
          u;
        l = (d * f + q) / e;
        m = (d * g + r) / e;
        n = (d * f + s) / e;
        o = (d * g + t) / e;
        u = (o - m) * (n - f) / (n - l) + g - o;
        m += u;
        o += u;
        if (m > r && m > g) {
          m = h(r, g);
          o = 2 * g - m
        } else if (m < r && m < g) {
          m = i(r, g);
          o = 2 * g - m
        }
        if (o > t && o > g) {
          o = h(t, g);
          m = 2 * g - o
        } else if (o < t && o < g) {
          o = i(t, g);
          m = 2 * g - o
        }
        b.rightContX = n;
        b.rightContY = o
      }
      if (!c) {
        p = [O, f, g]
      } else {
        p = ["C", j.rightContX || j.plotX, j.rightContY || j.plotY, l || f, m || g, f, g];
        j.rightContX = j.rightContY = null
      }
      return p
    }
  });
  bD.spline = cD;
  var cE = bU(cD, {
    type: "areaspline",
    useThreshold: true
  });
  bD.areaspline = cE;
  var cF = bU(cA, {
    type: "column",
    useThreshold: true,
    pointAttrToOptions: {
      stroke: "borderColor",
      "stroke-width": "borderWidth",
      fill: "color",
      r: "borderRadius"
    },
    init: function () {
      cA.prototype.init.apply(this, arguments);
      var a = this,
        b = a.chart;
      if (b.hasRendered) {
        bt(b.series, function (b) {
          if (b.type === a.type) {
            b.isDirty = true
          }
        })
      }
    },
    translate: function () {
      var b = this,
        c = b.chart,
        d = b.options,
        e = d.stacking,
        f = d.borderWidth,
        k = 0,
        l = b.xAxis,
        m = l.reversed,
        n = {},
        o, p;
      cA.prototype.translate.apply(b);
      bt(c.series, function (c) {
        if (c.type === b.type && c.visible && b.options.group === c.options.group) {
          if (c.options.stacking) {
            o = c.stackKey;
            if (n[o] === a) {
              n[o] = k++
            }
            p = n[o]
          } else {
            p = k++
          }
          c.columnIndex = p
        }
      });
      var q = b.points,
        r = j(l.translationSlope) * (l.ordinalSlope || l.closestPointRange),
        s = r * d.groupPadding,
        t = r - 2 * s,
        u = t / k,
        v = d.pointWidth,
        w = bO(v) ? (u - v) / 2 : u * d.pointPadding,
        x = g(h(bR(v, u - 2 * w), 1)),
        y = (m ? k - b.columnIndex : b.columnIndex) || 0,
        z = w + (s + y * u - r / 2) * (m ? -1 : 1),
        A = d.threshold,
        B = b.yAxis.getThreshold(A),
        C = bR(d.minPointLength, 5);
      bt(q, function (a) {
        var k = a.plotY,
          l = a.yBottom || B,
          m = a.plotX + z,
          n = g(i(k, l)),
          o = g(h(k, l) - n),
          p = b.yAxis.stacks[(a.y < 0 ? "-" : "") + b.stackKey],
          q, r;
        if (e && b.visible && p && p[a.x]) {
          p[a.x].setOffset(z, x)
        }
        if (j(o) < C) {
          if (C) {
            o = C;
            n = j(n - B) > C ? l - C : B - (k <= B ? C : 0)
          }
          q = n - 3
        }
        bE(a, {
          barX: m,
          barY: n,
          barW: x,
          barH: o
        });
        a.shapeType = "rect";
        r = bE(c.renderer.Element.prototype.crisp.apply({}, [f, m, n, x, o]), {
          r: d.borderRadius
        });
        if (f % 2) {
          r.y -= 1;
          r.height += 1
        }
        a.shapeArgs = r;
        a.trackerArgs = bO(q) && bx(a.shapeArgs, {
          height: h(6, o + 3),
          y: q
        })
      })
    },
    getSymbol: function () { },
    drawGraph: function () { },
    drawPoints: function () {
      var b = this,
        c = b.options,
        d = b.chart.renderer,
        e, f;
      bt(b.points, function (g) {
        var h = g.plotY;
        if (h !== a && !isNaN(h) && g.y !== null) {
          e = g.graphic;
          f = g.shapeArgs;
          if (e) {
            bC(e);
            e.animate(f)
          } else {
            g.graphic = e = d[g.shapeType](f).attr(g.pointAttr[g.selected ? T : R]).add(b.group).shadow(c.shadow)
          }
        }
      })
    },
    drawTracker: function () {
      var a = this,
        b = a.chart,
        c = b.renderer,
        d, e, f = +(new Date),
        g = a.options,
        h = g.cursor,
        i = h && {
          cursor: h
        },
        j;
      bt(a.points, function (h) {
        e = h.tracker;
        d = h.trackerArgs || h.shapeArgs;
        delete d.strokeWidth;
        if (h.y !== null) {
          if (e) {
            e.attr(d)
          } else {
            h.tracker = c[h.shapeType](d).attr({
              isTracker: f,
              fill: Q,
              visibility: a.visible ? L : J,
              zIndex: g.zIndex || 1
            }).on(x ? "touchstart" : "mouseover", function (c) {
              j = c.relatedTarget || c.fromElement;
              if (b.hoverSeries !== a && bP(j, "isTracker") !== f) {
                a.onMouseOver()
              }
              h.onMouseOver()
            }).on("mouseout", function (b) {
              if (!g.stickyTracking) {
                j = b.relatedTarget || b.toElement;
                if (bP(j, "isTracker") !== f) {
                  a.onMouseOut()
                }
              }
            }).css(i).add(h.group || b.trackerGroup)
          }
        }
      })
    },
    animate: function (a) {
      var b = this,
        c = b.points;
      if (!a) {
        bt(c, function (a) {
          var c = a.graphic,
            d = a.shapeArgs;
          if (c) {
            c.attr({
              height: 0,
              y: b.yAxis.translate(0, 0, 1)
            });
            c.animate({
              height: d.height,
              y: d.y
            }, b.options.animation)
          }
        });
        b.animate = null
      }
    },
    remove: function () {
      var a = this,
        b = a.chart;
      if (b.hasRendered) {
        bt(b.series, function (b) {
          if (b.type === a.type) {
            b.isDirty = true
          }
        })
      }
      cA.prototype.remove.apply(a, arguments)
    }
  });
  bD.column = cF;
  var cG = bU(cF, {
    type: "bar",
    init: function () {
      this.inverted = true;
      cF.prototype.init.apply(this, arguments)
    }
  });
  bD.bar = cG;
  var cH = bU(cA, {
    type: "scatter",
    translate: function () {
      var a = this;
      cA.prototype.translate.apply(a);
      bt(a.points, function (b) {
        b.shapeType = "circle";
        b.shapeArgs = {
          x: b.plotX,
          y: b.plotY,
          r: a.chart.options.tooltip.snap
        }
      })
    },
    drawTracker: function () {
      var a = this,
        b = a.options.cursor,
        c = b && {
          cursor: b
        },
        d;
      bt(a.points, function (b) {
        d = b.graphic;
        if (d) {
          d.attr({
            isTracker: true
          }).on("mouseover", function () {
            a.onMouseOver();
            b.onMouseOver()
          }).on("mouseout", function () {
            if (!a.options.stickyTracking) {
              a.onMouseOut()
            }
          }).css(c)
        }
      })
    }
  });
  bD.scatter = cH;
  var cI = bU(cz, {
    init: function () {
      cz.prototype.init.apply(this, arguments);
      var a = this,
        b;
      bE(a, {
        visible: a.visible !== false,
        name: bR(a.name, "Slice")
      });
      b = function () {
        a.slice()
      };
      by(a, "select", b);
      by(a, "unselect", b);
      return a
    },
    setVisible: function (b) {
      var c = this,
        d = c.series.chart,
        e = c.tracker,
        f = c.dataLabel,
        g = c.connector,
        h = c.shadowGroup,
        i;
      c.visible = b = b === a ? !c.visible : b;
      i = b ? "show" : "hide";
      c.group[i]();
      if (e) {
        e[i]()
      }
      if (f) {
        f[i]()
      }
      if (g) {
        g[i]()
      }
      if (h) {
        h[i]()
      }
      if (c.legendItem) {
        d.legend.colorizeItem(c, b)
      }
    },
    slice: function (a, b, c) {
      var d = this,
        e = d.series,
        f = e.chart,
        g = d.slicedTranslation,
        h;
      cc(c, f);
      b = bR(b, true);
      a = d.sliced = bO(a) ? a : !d.sliced;
      h = {
        translateX: a ? g[0] : f.plotLeft,
        translateY: a ? g[1] : f.plotTop
      };
      d.group.animate(h);
      if (d.shadowGroup) {
        d.shadowGroup.animate(h)
      }
    }
  });
  var cJ = bU(cA, {
    type: "pie",
    isCartesian: false,
    pointClass: cI,
    pointAttrToOptions: {
      stroke: "borderColor",
      "stroke-width": "borderWidth",
      fill: "color"
    },
    getColor: function () {
      this.initialColor = this.chart.counters.color
    },
    animate: function () {
      var a = this,
        b = a.points;
      bt(b, function (b) {
        var c = b.graphic,
          d = b.shapeArgs,
          e = -m / 2;
        if (c) {
          c.attr({
            r: 0,
            start: e,
            end: e
          });
          c.animate({
            r: d.r,
            start: d.start,
            end: d.end
          }, a.options.animation)
        }
      });
      a.animate = null
    },
    setData: function () {
      cA.prototype.setData.apply(this, arguments);
      this.processData();
      this.generatePoints()
    },
    translate: function () {
      this.generatePoints();
      var a = 0,
        b = this,
        c = -.25,
        f = 1e3,
        g = b.options,
        h = g.slicedOffset,
        j = h + g.borderWidth,
        n = g.center.concat([g.size, g.innerSize || 0]),
        o = b.chart,
        p = o.plotWidth,
        q = o.plotHeight,
        r, s, t, u = b.points,
        v = 2 * m,
        w, x = i(p, q),
        y, z, A, B = g.dataLabels.distance;
      n = bw(n, function (a, b) {
        y = /%$/.test(a);
        return y ? [p, q, x, x][b] * bG(a) / 100 : a
      });
      b.getX = function (a, b) {
        t = d.asin((a - n[1]) / (n[2] / 2 + B));
        return n[0] + (b ? -1 : 1) * k(t) * (n[2] / 2 + B)
      };
      b.center = n;
      bt(u, function (b) {
        a += b.y
      });
      bt(u, function (b) {
        w = a ? b.y / a : 0;
        r = e(c * v * f) / f;
        c += w;
        s = e(c * v * f) / f;
        b.shapeType = "arc";
        b.shapeArgs = {
          x: n[0],
          y: n[1],
          r: n[2] / 2,
          innerR: n[3] / 2,
          start: r,
          end: s
        };
        t = (s + r) / 2;
        b.slicedTranslation = bw([k(t) * h + o.plotLeft, l(t) * h + o.plotTop], e);
        z = k(t) * n[2] / 2;
        A = l(t) * n[2] / 2;
        b.tooltipPos = [n[0] + z * .7, n[1] + A * .7];
        b.labelPos = [n[0] + z + k(t) * B, n[1] + A + l(t) * B, n[0] + z + k(t) * j, n[1] + A + l(t) * j, n[0] + z, n[1] + A, B < 0 ? "center" : t < v / 4 ? "left" : "right", t];
        b.percentage = w * 100;
        b.total = a
      });
      this.setTooltipPoints()
    },
    render: function () {
      var a = this;
      a.getAttribs();
      this.drawPoints();
      if (a.options.enableMouseTracking !== false) {
        a.drawTracker()
      }
      this.drawDataLabels();
      if (a.options.animation && a.animate) {
        a.animate()
      }
      a.isDirty = false
    },
    drawPoints: function () {
      var a = this,
        b = a.chart,
        c = b.renderer,
        d, e, f, g = a.options.shadow,
        h, i;
      bt(a.points, function (a) {
        e = a.graphic;
        i = a.shapeArgs;
        f = a.group;
        h = a.shadowGroup;
        if (g && !h) {
          h = a.shadowGroup = c.g("shadow").attr({
            zIndex: 4
          }).add()
        }
        if (!f) {
          f = a.group = c.g("point").attr({
            zIndex: 5
          }).add()
        }
        d = a.sliced ? a.slicedTranslation : [b.plotLeft, b.plotTop];
        f.translate(d[0], d[1]);
        if (h) {
          h.translate(d[0], d[1])
        }
        if (e) {
          e.animate(i)
        } else {
          a.graphic = c.arc(i).attr(bE(a.pointAttr[R], {
            "stroke-linejoin": "round"
          })).add(a.group).shadow(g, h)
        }
        if (a.visible === false) {
          a.setVisible(false)
        }
      })
    },
    drawDataLabels: function () {
      var a = this,
        b = a.data,
        c, d = a.chart,
        e = a.options.dataLabels,
        f = bR(e.connectorPadding, 10),
        g = bR(e.connectorWidth, 1),
        h, i, k = bR(e.softConnector, true),
        l = e.distance,
        n = a.center,
        o = n[2] / 2,
        p = n[1],
        q = l > 0,
        r, s, t, u = [
          [],
          []
        ],
        v, w, x, y, z, A = 2,
        B;
      if (!e.enabled) {
        return
      }
      cA.prototype.drawDataLabels.apply(a);
      bt(b, function (a) {
        if (a.dataLabel) {
          u[a.labelPos[7] < m / 2 ? 0 : 1].push(a)
        }
      });
      u[1].reverse();
      z = function (a, b) {
        return b.y - a.y
      };
      t = u[0][0] && u[0][0].dataLabel && bG(u[0][0].dataLabel.styles.lineHeight);
      while (A--) {
        var C = [],
          D, E = [],
          F = u[A],
          G, H = F.length,
          I;
        for (G = p - o - l; G <= p + o + l; G += t) {
          C.push(G)
        }
        D = C.length;
        if (H > D) {
          y = [].concat(F);
          y.sort(z);
          B = H;
          while (B--) {
            y[B].rank = B
          }
          B = H;
          while (B--) {
            if (F[B].rank >= D) {
              F.splice(B, 1)
            }
          }
          H = F.length
        }
        for (B = 0; B < H; B++) {
          c = F[B];
          s = c.labelPos;
          var K = 9999,
            M, N;
          for (N = 0; N < D; N++) {
            M = j(C[N] - s[1]);
            if (M < K) {
              K = M;
              I = N
            }
          }
          if (I < B && C[B] !== null) {
            I = B
          } else if (D < H - B + I && C[B] !== null) {
            I = D - H + B;
            while (C[I] === null) {
              I++
            }
          } else {
            while (C[I] === null) {
              I++
            }
          }
          E.push({
            i: I,
            y: C[I]
          });
          C[I] = null
        }
        E.sort(z);
        for (B = 0; B < H; B++) {
          c = F[B];
          s = c.labelPos;
          r = c.dataLabel;
          var Q = E.pop(),
            R = s[1];
          x = c.visible === false ? J : L;
          I = Q.i;
          w = Q.y;
          if (R > w && C[I + 1] !== null || R < w && C[I - 1] !== null) {
            w = R
          }
          v = a.getX(I === 0 || I === C.length - 1 ? R : w, A);
          r.attr({
            visibility: x,
            align: s[6]
          })[r.moved ? "animate" : "attr"]({
            x: v + e.x + ({
              left: f,
              right: -f
            }[s[6]] || 0),
            y: w + e.y
          });
          r.moved = true;
          if (q && g) {
            h = c.connector;
            i = k ? [O, v + (s[6] === "left" ? 5 : -5), w, "C", v, w, 2 * s[2] - s[4], 2 * s[3] - s[5], s[2], s[3], P, s[4], s[5]] : [O, v + (s[6] === "left" ? 5 : -5), w, P, s[2], s[3], P, s[4], s[5]];
            if (h) {
              h.animate({
                d: i
              });
              h.attr("visibility", x)
            } else {
              c.connector = h = a.chart.renderer.path(i).attr({
                "stroke-width": g,
                stroke: e.connectorColor || c.color || "#606060",
                visibility: x,
                zIndex: 3
              }).translate(d.plotLeft, d.plotTop).add()
            }
          }
        }
      }
    },
    drawTracker: cF.prototype.drawTracker,
    getSymbol: function () { }
  });
  bD.pie = cJ;
  var cK = "dataGrouping",
    cL = cA.prototype,
    cM = cL.processData,
    cN = cL.generatePoints,
    cO = cL.destroy,
    cP = cL.tooltipHeaderFormatter,
    cQ = "number",
    cR = {
      approximation: "average",
      groupPixelWidth: 2,
      dateTimeLabelFormats: bF(U, ["%A, %b %e, %H:%M:%S.%L", "%A, %b %e, %H:%M:%S.%L", "-%H:%M:%S.%L"], V, ["%A, %b %e, %H:%M:%S", "%A, %b %e, %H:%M:%S", "-%H:%M:%S"], W, ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], X, ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], Y, ["%A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], Z, ["Week from %A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], $, ["%B %Y", "%B", "-%B %Y"], _, ["%Y", "%Y", "-%Y"])
    },
    cS = [
      [U, [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]],
      [V, [1, 2, 5, 10, 15, 30]],
      [W, [1, 2, 5, 10, 15, 30]],
      [X, [1, 2, 3, 4, 6, 8, 12]],
      [Y, [1]],
      [Z, [1]],
      [$, [1, 3, 6]],
      [_, null]
    ],
    cT = {
      sum: function (a) {
        var b = a.length,
          c;
        if (!b && a.hasNulls) {
          c = null
        } else if (b) {
          c = 0;
          while (b--) {
            c += a[b]
          }
        }
        return c
      },
      average: function (a) {
        var b = a.length,
          c = cT.sum(a);
        if (typeof c === cQ && b) {
          c = c / b
        }
        return c
      },
      open: function (b) {
        return b.length ? b[0] : b.hasNulls ? null : a
      },
      high: function (b) {
        return b.length ? ca(b) : b.hasNulls ? null : a
      },
      low: function (b) {
        return b.length ? b_(b) : b.hasNulls ? null : a
      },
      close: function (b) {
        return b.length ? b[b.length - 1] : b.hasNulls ? null : a
      },
      ohlc: function (a, b, c, d) {
        a = cT.open(a);
        b = cT.high(b);
        c = cT.low(c);
        d = cT.close(d);
        if (typeof a === cQ || typeof b === cQ || typeof c === cQ || typeof d === cQ) {
          return [a, b, c, d]
        }
      }
    };
  cL.groupData = function (b, c, d, e) {
    var f = this,
      g = f.data,
      h = f.options.data,
      i = [],
      j = [],
      k = b.length,
      l, m, n, o = !!c,
      p = [],
      q = [],
      r = [],
      s = [],
      t = typeof e === "function" ? e : cT[e],
      u;
    for (u = 0; u <= k; u++) {
      while (d[1] !== a && b[u] >= d[1] || u === k) {
        l = d.shift();
        n = t(p, q, r, s);
        if (n !== a) {
          i.push(l);
          j.push(n)
        }
        p = [];
        q = [];
        r = [];
        s = [];
        if (u === k) {
          break
        }
      }
      if (u === k) {
        break
      }
      m = o ? c[u] : null;
      if (e === "ohlc") {
        var v = f.cropStart + u,
          w = g && g[v] || f.pointClass.prototype.applyOptions.apply({}, [h[v]]),
          x = w.open,
          y = w.high,
          z = w.low,
          A = w.close;
        if (typeof x === cQ) {
          p.push(x)
        } else if (x === null) {
          p.hasNulls = true
        }
        if (typeof y === cQ) {
          q.push(y)
        } else if (y === null) {
          q.hasNulls = true
        }
        if (typeof z === cQ) {
          r.push(z)
        } else if (z === null) {
          r.hasNulls = true
        }
        if (typeof A === cQ) {
          s.push(A)
        } else if (A === null) {
          s.hasNulls = true
        }
      } else {
        if (typeof m === cQ) {
          p.push(m)
        } else if (m === null) {
          p.hasNulls = true
        }
      }
    }
    return [i, j]
  };
  cL.processData = function () {
    var a = this,
      b = a.options,
      c = b[cK],
      d = c && c.enabled,
      e;
    a.forceCrop = d;
    if (cM.apply(a) === false || !d) {
      return
    }
    var f, g = a.chart,
      i = a.processedXData,
      j = a.processedYData,
      k = g.plotSizeX,
      l = a.xAxis,
      m = bR(l.groupPixelWidth, c.groupPixelWidth),
      n = k / m,
      o = i.length,
      p = a.groupedData,
      q = g.series;
    if (!l.groupPixelWidth) {
      f = q.length;
      while (f--) {
        if (q[f].xAxis === l && q[f].options[cK]) {
          m = h(m, q[f].options[cK].groupPixelWidth)
        }
      }
      l.groupPixelWidth = m
    }
    bt(p || [], function (a, b) {
      if (a) {
        p[b] = a.destroy ? a.destroy() : null
      }
    });
    if (o > n || c.forced) {
      e = true;
      a.points = null;
      var r = l.getExtremes(),
        s = r.min,
        t = r.max,
        u = l.options.ordinal ? k * ((t - s) / (o * a.closestPointRange)) : k,
        v = m * (t - s) / u,
        w = bX(v, s, t, null, c.units || cS),
        x = cL.groupData.apply(a, [i, j, w, c.approximation]),
        y = x[0],
        z = x[1];
      if (c.smoothed) {
        f = y.length - 1;
        y[f] = t;
        while (f-- && f > 0) {
          y[f] += v / 2
        }
        y[0] = s
      }
      a.currentDataGrouping = w.info;
      if (b.pointRange === null) {
        a.pointRange = w.info.totalRange
      }
      a.closestPointRange = w.info.totalRange;
      a.processedXData = y;
      a.processedYData = z
    } else {
      a.currentDataGrouping = null;
      a.pointRange = b.pointRange
    }
    a.hasGroupedData = e
  };
  cL.generatePoints = function () {
    var a = this;
    cN.apply(a);
    a.groupedData = a.hasGroupedData ? a.points : null
  };
  cL.tooltipHeaderFormatter = function (a) {
    var b = this,
      c = b.options,
      d = b.tooltipOptions,
      e = c.dataGrouping,
      f = d.xDateFormat,
      g, h = b.xAxis,
      i, j, k, l, m, n;
    if (h && h.options.type === "datetime" && e) {
      i = b.currentDataGrouping;
      j = e.dateTimeLabelFormats;
      if (i) {
        k = j[i.unitName];
        if (i.count === 1) {
          f = k[0]
        } else {
          f = k[1];
          g = k[2]
        }
      } else if (!f) {
        for (m in F) {
          if (F[m] >= h.closestPointRange) {
            f = j[m][0];
            break
          }
        }
      }
      l = C(f, a);
      if (g) {
        l += C(g, a + i.totalRange - 1)
      }
      n = d.headerFormat.replace("{point.key}", l)
    } else {
      n = cP.apply(b, [a])
    }
    return n
  };
  cL.destroy = function () {
    var a = this,
      b = a.groupedData || [],
      c = b.length;
    while (c--) {
      if (b[c]) {
        b[c].destroy()
      }
    }
    cO.apply(a)
  };
  cr.line[cK] = cr.spline[cK] = cr.area[cK] = cr.areaspline[cK] = cR;
  cr.column[cK] = bx(cR, {
    approximation: "sum",
    groupPixelWidth: 10
  });
  cr.ohlc = bx(cr.column, {
    lineWidth: 1,
    dataGrouping: {
      approximation: "ohlc",
      enabled: true,
      groupPixelWidth: 5
    },
    states: {
      hover: {
        lineWidth: 3
      }
    }
  });
  var cU = bU(cz, {
    applyOptions: function (b) {
      var c = this,
        d = c.series,
        e = 0;
      if (typeof b === "object" && typeof b.length !== "number") {
        bE(c, b);
        c.options = b
      } else if (b.length) {
        if (b.length === 5) {
          if (typeof b[0] === "string") {
            c.name = b[0]
          } else if (typeof b[0] === "number") {
            c.x = b[0]
          }
          e++
        }
        c.open = b[e++];
        c.high = b[e++];
        c.low = b[e++];
        c.close = b[e++]
      }
      c.y = c.high;
      if (c.x === a && d) {
        c.x = d.autoIncrement()
      }
      return c
    },
    tooltipFormatter: function () {
      var a = this,
        b = a.series;
      return ['<span style="color:' + b.color + ';font-weight:bold">', a.name || b.name, "</span><br/>", "Open: ", a.open, "<br/>", "High: ", a.high, "<br/>", "Low: ", a.low, "<br/>", "Close: ", a.close, "<br/>"].join("")
    }
  });
  var cV = bU(bD.column, {
    type: "ohlc",
    valueCount: 4,
    pointClass: cU,
    useThreshold: false,
    pointAttrToOptions: {
      stroke: "color",
      "stroke-width": "lineWidth"
    },
    translate: function () {
      var a = this,
        b = a.yAxis;
      bD.column.prototype.translate.apply(a);
      bt(a.points, function (a) {
        if (a.open !== null) {
          a.plotOpen = b.translate(a.open, 0, 1, 0, 1)
        }
        if (a.close !== null) {
          a.plotClose = b.translate(a.close, 0, 1, 0, 1)
        }
      })
    },
    drawPoints: function () {
      var b = this,
        c = b.points,
        d = b.chart,
        f, g, h, i, j, k, l, m;
      bt(c, function (c) {
        if (c.plotY !== a) {
          l = c.graphic;
          f = c.pointAttr[c.selected ? "selected" : ""];
          i = f["stroke-width"] % 2 / 2;
          m = e(c.plotX) + i;
          j = e(c.barW / 2);
          k = ["M", m, e(c.yBottom), "L", m, e(c.plotY)];
          if (c.open !== null) {
            g = e(c.plotOpen) + i;
            k.push("M", m, g, "L", m - j, g)
          }
          if (c.close !== null) {
            h = e(c.plotClose) + i;
            k.push("M", m, h, "L", m + j, h)
          }
          if (l) {
            l.animate({
              d: k
            })
          } else {
            c.graphic = d.renderer.path(k).attr(f).add(b.group)
          }
        }
      })
    },
    animate: null
  });
  bD.ohlc = cV;
  cr.candlestick = bx(cr.column, {
    dataGrouping: {
      approximation: "ohlc",
      enabled: true
    },
    lineColor: "black",
    lineWidth: 1,
    upColor: "white",
    states: {
      hover: {
        lineWidth: 2
      }
    }
  });
  var cW = bU(cV, {
    type: "candlestick",
    pointAttrToOptions: {
      fill: "color",
      stroke: "lineColor",
      "stroke-width": "lineWidth"
    },
    getAttribs: function () {
      cV.prototype.getAttribs.apply(this, arguments);
      var a = this,
        b = a.options,
        c = b.states,
        d = b.upColor,
        e = bx(a.pointAttr);
      e[""].fill = d;
      e.hover.fill = c.hover.upColor || d;
      e.select.fill = c.select.upColor || d;
      bt(a.points, function (a) {
        if (a.open < a.close) {
          a.pointAttr = e
        }
      })
    },
    drawPoints: function () {
      var b = this,
        c = b.points,
        f = b.chart,
        g, h, i, j, k, l, m, n, o, p;
      bt(c, function (c) {
        n = c.graphic;
        if (c.plotY !== a) {
          g = c.pointAttr[c.selected ? "selected" : ""];
          l = g["stroke-width"] % 2 / 2;
          m = e(c.plotX) + l;
          h = e(c.plotOpen) + l;
          i = e(c.plotClose) + l;
          j = d.min(h, i);
          k = d.max(h, i);
          p = e(c.barW / 2);
          o = ["M", m - p, k, "L", m - p, j, "L", m + p, j, "L", m + p, k, "L", m - p, k, "M", m, k, "L", m, e(c.yBottom), "M", m, j, "L", m, e(c.plotY), "Z"];
          if (n) {
            n.animate({
              d: o
            })
          } else {
            c.graphic = f.renderer.path(o).attr(g).add(b.group)
          }
        }
      })
    }
  });
  bD.candlestick = cW;
  var cX = cv.prototype.symbols;
  cr.flags = bx(cr.column, {
    fillColor: "white",
    lineWidth: 1,
    shape: "flag",
    stackDistance: 7,
    states: {
      hover: {
        lineColor: "black",
        fillColor: "#FCFFC5"
      }
    },
    style: {
      fontSize: "11px",
      fontWeight: "bold",
      textAlign: "center"
    },
    y: -30
  });
  bD.flags = bU(bD.column, {
    type: "flags",
    noSharedTooltip: true,
    useThreshold: false,
    init: cA.prototype.init,
    pointAttrToOptions: {
      fill: "fillColor",
      stroke: "color",
      "stroke-width": "lineWidth",
      r: "radius"
    },
    translate: function () {
      bD.column.prototype.translate.apply(this);
      var b = this,
        c = b.options,
        d = b.chart,
        e = b.points,
        f = e.length - 1,
        g, h, i, j = c.onSeries,
        k = j && d.get(j),
        l, m;
      if (k) {
        l = k.points;
        g = l.length;
        e.sort(function (a, b) {
          return a.x - b.x
        });
        while (g-- && e[f]) {
          h = e[f];
          m = l[g];
          if (m.x <= h.x) {
            h.plotY = m.plotY;
            f--;
            g++;
            if (f < 0) {
              break
            }
          }
        }
      }
      bt(e, function (b, c) {
        if (!k) {
          b.plotY = b.y === a ? d.plotHeight : b.plotY
        }
        i = e[c - 1];
        if (i && i.plotX === b.plotX) {
          if (i.stackIndex === a) {
            i.stackIndex = 0
          }
          b.stackIndex = i.stackIndex + 1
        }
      })
    },
    drawPoints: function () {
      var b = this,
        c, d = b.points,
        e = b.chart,
        f = e.renderer,
        g, h, i = b.options,
        j = i.y,
        k = i.shape,
        l, m, n, o, p, q, r, s = i.lineWidth % 2 / 2,
        t, u;
      n = d.length;
      while (n--) {
        o = d[n];
        g = o.plotX + s;
        r = o.stackIndex;
        h = o.plotY + j + s - (r !== a && r * i.stackDistance);
        if (isNaN(h)) {
          h = 0
        }
        t = r ? a : o.plotX + s;
        u = r ? a : o.plotY;
        p = o.graphic;
        q = o.connector;
        if (h !== a) {
          c = o.pointAttr[o.selected ? "select" : ""];
          if (p) {
            p.attr({
              x: g,
              y: h,
              r: c.r,
              anchorX: t,
              anchorY: u
            })
          } else {
            p = o.graphic = f.label(o.options.title || i.title || "A", g, h, k, t, u).css(bx(i.style, o.style)).attr(c).attr({
              align: k === "flag" ? "left" : "center",
              width: i.width,
              height: i.height
            }).add(b.group).shadow(i.shadow)
          }
          l = p.box;
          m = l.getBBox();
          o.shapeArgs = bE(m, {
            x: g - (k === "flag" ? 0 : l.attr("width") / 2),
            y: h
          })
        }
      }
    },
    drawTracker: function () {
      var a = this;
      bD.column.prototype.drawTracker.apply(a);
      bt(a.points, function (a) {
        by(a.tracker.element, "mouseover", function () {
          a.graphic.toFront()
        })
      })
    },
    tooltipFormatter: function (a) {
      return a.point.text
    },
    animate: function () { }
  });
  cX.flag = function (a, b, c, d, e) {
    var f = e && e.anchorX || a,
      g = e && e.anchorY || b;
    return ["M", f, g, "L", a, b + d, a, b, a + c, b, a + c, b + d, a, b + d, "M", f, g, "Z"]
  };
  bt(["circle", "square"], function (a) {
    cX[a + "pin"] = function (b, c, d, e, f) {
      var g = f && f.anchorX,
        h = f && f.anchorY,
        i = cX[a](b, c, d, e);
      if (g && h) {
        i.push("M", g, c + e, "L", g, h)
      }
      return i
    }
  });
  if (w === cw) {
    bt(["flag", "circlepin", "squarepin"], function (a) {
      cw.prototype.symbols[a] = cX[a]
    })
  }
  var cY = x ? "touchstart" : "mousedown",
    cZ = x ? "touchmove" : "mousemove",
    c$ = x ? "touchend" : "mouseup";
  var c_ = bF(bb, {
    x1: 0,
    y1: 0,
    x2: 0,
    y2: 1
  }, bc, [
      [0, "#FFF"],
      [1, "#CCC"]
    ]),
    da = [].concat(cS);
  da[4] = [Y, [1, 2, 3, 4]];
  da[5] = [Z, [1, 2, 3]];
  bE(B, {
    navigator: {
      handles: {
        backgroundColor: "#FFF",
        borderColor: "#666"
      },
      height: 40,
      margin: 10,
      maskFill: "rgba(255, 255, 255, 0.75)",
      outlineColor: "#444",
      outlineWidth: 1,
      series: {
        type: "areaspline",
        color: "#4572A7",
        compare: null,
        fillOpacity: .4,
        dataGrouping: {
          approximation: "average",
          groupPixelWidth: 2,
          smoothed: true,
          units: da
        },
        dataLabels: {
          enabled: false
        },
        id: K + "navigator-series",
        lineColor: "#4572A7",
        lineWidth: 1,
        marker: {
          enabled: false
        },
        pointRange: 0,
        shadow: false
      },
      xAxis: {
        tickWidth: 0,
        lineWidth: 0,
        gridLineWidth: 1,
        tickPixelInterval: 200,
        labels: {
          align: "left",
          x: 3,
          y: -4
        }
      },
      yAxis: {
        gridLineWidth: 0,
        startOnTick: false,
        endOnTick: false,
        minPadding: .1,
        maxPadding: .1,
        labels: {
          enabled: false
        },
        title: {
          text: null
        },
        tickWidth: 0
      }
    },
    scrollbar: {
      height: x ? 20 : 14,
      barBackgroundColor: c_,
      barBorderRadius: 2,
      barBorderWidth: 1,
      barBorderColor: "#666",
      buttonArrowColor: "#666",
      buttonBackgroundColor: c_,
      buttonBorderColor: "#666",
      buttonBorderRadius: 2,
      buttonBorderWidth: 1,
      rifleColor: "#666",
      trackBackgroundColor: bF(bb, {
        x1: 0,
        y1: 0,
        x2: 0,
        y2: 1
      }, bc, [
          [0, "#EEE"],
          [1, "#FFF"]
        ]),
      trackBorderColor: "#CCC",
      trackBorderWidth: 1
    }
  });
  Highcharts.Scroller = function (a) {
    function bn() {
      bl();
      bt([v, w, V, W, X, $, _, ba, Z], function (a) {
        if (a && a.destroy) {
          a.destroy()
        }
      });
      v = w = V = W = X = $ = _ = ba = Z = null;
      bt([bb, Y, bc], function (a) {
        cb(a)
      })
    }

    function bm() {
      var b = a.xAxis.length,
        c = a.yAxis.length;
      a.extraBottomMargin = H + f.margin;
      if (g) {
        var d = U.options,
          e, h = d.data,
          i = f.series;
        m = i.data;
        d.data = i.data = null;
        v = new a.Axis(bx({
          ordinal: U.xAxis.options.ordinal
        }, f.xAxis, {
            isX: true,
            type: "datetime",
            index: b,
            height: E,
            top: K,
            offset: 0,
            offsetLeft: G,
            offsetRight: -G,
            startOnTick: false,
            endOnTick: false,
            minPadding: 0,
            maxPadding: 0,
            zoomEnabled: false
          }));
        w = new a.Axis(bx(f.yAxis, {
          alignTicks: false,
          height: E,
          top: K,
          offset: 0,
          index: c,
          zoomEnabled: false
        }));
        e = bx(U.options, i, {
          threshold: null,
          clip: false,
          enableMouseTracking: false,
          group: "nav",
          padXAxis: false,
          xAxis: b,
          yAxis: c,
          name: "Navigator",
          showInLegend: false,
          isInternal: true,
          visible: true
        });
        d.data = h;
        i.data = m;
        e.data = m || h;
        l = a.initSeries(e);
        by(U, "updatedData", bj)
      } else {
        v = {
          translate: function (b, c) {
            var d = U.xAxis.getExtremes(),
              e = a.plotWidth - 2 * G,
              f = d.dataMin,
              g = d.dataMax - f;
            return c ? b * g / e + f : e * (b - f) / g
          }
        }
      }
      bk()
    }

    function bl() {
      bz(a.container, cY, bg);
      bz(a.container, cZ, bh);
      bz(document, c$, bi);
      if (g) {
        bz(U, "updatedData", bj)
      }
    }

    function bk() {
      by(a.container, cY, bg);
      by(a.container, cZ, bh);
      by(document, c$, bi)
    }

    function bj() {
      var b = U.xAxis,
        c = b.getExtremes(),
        d = c.min,
        e = c.max,
        f = c.dataMin,
        g = c.dataMax,
        j = e - d,
        k, n, o, p, q, r = l.xData,
        s = !!b.setExtremes;
      n = e >= r[r.length - 1];
      k = d <= f;
      if (!m) {
        l.options.pointStart = U.xData[0];
        l.setData(U.options.data, false);
        q = true
      }
      if (k) {
        p = f;
        o = p + j
      }
      if (n) {
        o = g;
        if (!k) {
          p = h(o - j, l.xData[0])
        }
      }
      if (s && (k || n)) {
        b.setExtremes(p, o, true, false)
      } else {
        if (q) {
          a.redraw(false)
        }
        bf(h(d, f), i(e, g))
      }
    }

    function bi() {
      if (u) {
        a.xAxis[0].setExtremes(v.translate(y, true), v.translate(z, true), true, false)
      }
      p = q = r = u = t = null;
      B.cursor = C
    }

    function bh(b) {
      b = a.tracker.normalizeMouseEvent(b);
      var c = b.chartX;
      if (c < j) {
        c = j
      } else if (c > Q + R - G) {
        c = Q + R - G
      }
      if (p) {
        u = true;
        bf(0, 0, c - j, s)
      } else if (q) {
        u = true;
        bf(0, 0, s, c - j)
      } else if (r) {
        u = true;
        if (c < t) {
          c = t
        } else if (c > k + t - A) {
          c = k + t - A
        }
        bf(0, 0, c - t, c - t + A)
      }
    }

    function bg(b) {
      b = a.tracker.normalizeMouseEvent(b);
      var c = b.chartX,
        e = b.chartY,
        f = x ? 10 : 7,
        g, h;
      if (e > K && e < K + E + G) {
        h = !o || e < K + E;
        if (h && d.abs(c - y - j) < f) {
          p = true;
          s = z
        } else if (h && d.abs(c - z - j) < f) {
          q = true;
          s = y
        } else if (c > j + y && c < j + z) {
          r = c;
          C = B.cursor;
          B.cursor = "ew-resize";
          t = c - y
        } else if (c > Q && c < Q + R) {
          if (h) {
            g = c - j - A / 2
          } else {
            if (c < j) {
              g = y - i(10, A)
            } else if (c > Q + R - G) {
              g = y + i(10, A)
            } else {
              g = c < j + y ? y - A : z
            }
          }
          if (g < 0) {
            g = 0
          } else if (g + A > k) {
            g = k - A
          }
          if (g !== y) {
            a.xAxis[0].setExtremes(v.translate(g, true), v.translate(g + A, true), true, false)
          }
        }
      }
      if (b.preventDefault) {
        b.preventDefault()
      }
    }

    function bf(c, d, l, m) {
      if (isNaN(c)) {
        return
      }
      var p, q = n.barBorderWidth,
        r;
      N = K + M;
      j = bR(v.left, a.plotLeft + G);
      k = bR(v.len, a.plotWidth - 2 * G);
      Q = j - G;
      R = k + 2 * G;
      if (v.getExtremes) {
        var s = a.xAxis[0].getExtremes(),
          t = s.dataMin === null,
          u = v.getExtremes(),
          w = i(s.dataMin, u.dataMin),
          x = h(s.dataMax, u.dataMax);
        if (!t && (w !== u.min || x !== u.max)) {
          v.setExtremes(w, x, true, false)
        }
      }
      l = bR(l, v.translate(c));
      m = bR(m, v.translate(d));
      y = bG(i(l, m));
      z = bG(h(l, m));
      A = z - y;
      if (!S) {
        if (g) {
          V = b.rect().attr({
            fill: f.maskFill,
            zIndex: 3
          }).add();
          W = b.rect().attr({
            fill: f.maskFill,
            zIndex: 3
          }).add();
          X = b.path().attr({
            "stroke-width": F,
            stroke: f.outlineColor,
            zIndex: 3
          }).add()
        }
        if (o) {
          Z = b.g().add();
          p = n.trackBorderWidth;
          $ = b.rect().attr({
            y: -p % 2 / 2,
            fill: n.trackBackgroundColor,
            stroke: n.trackBorderColor,
            "stroke-width": p,
            r: n.trackBorderRadius || 0,
            height: G
          }).add(Z);
          _ = b.rect().attr({
            y: -q % 2 / 2,
            height: G,
            fill: n.barBackgroundColor,
            stroke: n.barBorderColor,
            "stroke-width": q,
            r: I
          }).add(Z);
          ba = b.path().attr({
            stroke: n.rifleColor,
            "stroke-width": 1
          }).add(Z)
        }
      }
      if (g) {
        V.attr({
          x: j,
          y: K,
          width: y,
          height: E
        });
        W.attr({
          x: j + z,
          y: K,
          width: k - z,
          height: E
        });
        X.attr({
          d: [O, Q, N, P, j + y + M, N, j + y + M, N + H - G, O, j + z - M, N + H - G, P, j + z - M, N, Q + R, N]
        });
        bd(y + M, 0);
        bd(z + M, 1)
      }
      if (o) {
        be(0);
        be(1);
        Z.translate(Q, e(N + E));
        $.attr({
          width: R
        });
        _.attr({
          x: e(G + y) + q % 2 / 2,
          width: A - q
        });
        r = G + y + A / 2 - .5;
        ba.attr({
          d: [O, r - 3, G / 4, P, r - 3, 2 * G / 3, O, r, G / 4, P, r, 2 * G / 3, O, r + 3, G / 4, P, r + 3, 2 * G / 3],
          visibility: A > 12 ? L : J
        })
      }
      S = true
    }

    function be(a) {
      var c;
      if (!S) {
        bb[a] = b.g().add(Z);
        c = b.rect(-.5, -.5, G + 1, G + 1, n.buttonBorderRadius, n.buttonBorderWidth).attr({
          stroke: n.buttonBorderColor,
          "stroke-width": n.buttonBorderWidth,
          fill: n.buttonBackgroundColor
        }).add(bb[a]);
        bc.push(c);
        c = b.path(["M", G / 2 + (a ? -1 : 1), G / 2 - 3, "L", G / 2 + (a ? -1 : 1), G / 2 + 3, G / 2 + (a ? 2 : -2), G / 2]).attr({
          fill: n.buttonArrowColor
        }).add(bb[a]);
        bc.push(c)
      }
      if (a) {
        bb[a].attr({
          translateX: R - G
        })
      }
    }

    function bd(a, c) {
      var d = {
        fill: D.backgroundColor,
        stroke: D.borderColor,
        "stroke-width": 1
      },
        e;
      if (!S) {
        Y[c] = b.g().css({
          cursor: "e-resize"
        }).attr({
          zIndex: 3
        }).add();
        e = b.rect(-4.5, 0, 9, 16, 3, 1).attr(d).add(Y[c]);
        bc.push(e);
        e = b.path(["M", -1.5, 4, "L", -1.5, 12, "M", .5, 4, "L", .5, 12]).attr(d).add(Y[c]);
        bc.push(e)
      }
      Y[c].translate(Q + G + parseInt(a, 10), K + E / 2 - 8)
    }
    var b = a.renderer,
      c = a.options,
      f = c.navigator,
      g = f.enabled,
      j, k, l, m, n = c.scrollbar,
      o = n.enabled,
      p, q, r, s, t, u, v, w, y, z, A, B = document.body.style,
      C, D = f.handles,
      E = g ? f.height : 0,
      F = f.outlineWidth,
      G = o ? n.height : 0,
      H = E + G,
      I = n.barBorderRadius,
      K = f.top || a.chartHeight - E - G - c.chart.spacingBottom,
      M = F / 2,
      N, Q, R, S, T = f.baseSeries,
      U = a.series[T] || typeof T === "string" && a.get(T) || a.series[0],
      V, W, X, Y = [],
      Z, $, _, ba, bb = [],
      bc = [];
    a.resetZoomEnabled = false;
    bm();
    return {
      render: bf,
      destroy: bn
    }
  };
  bE(B, {
    rangeSelector: {
      buttonTheme: {
        width: 28,
        height: 16,
        padding: 1,
        r: 0,
        zIndex: 10
      }
    }
  });
  B.lang = bx(B.lang, {
    rangeSelectorZoom: "Zoom",
    rangeSelectorFrom: "From:",
    rangeSelectorTo: "To:"
  });
  Highcharts.RangeSelector = function (b) {
    function A() {
      bz(e, cY, v);
      bt([q], function (a) {
        cb(a)
      });
      if (p) {
        p = p.destroy()
      }
      if (j) {
        j.onfocus = j.onblur = j.onchange = null
      }
      if (k) {
        k.onfocus = k.onblur = k.onchange = null
      }
      bt([j, k, l.min, l.max, m, n], function (a) {
        cj(a)
      });
      j = k = l = g = m = n = null
    }

    function z(a, h) {
      var i = b.options.chart.style,
        l = s.buttonTheme,
        t = s.inputEnabled !== false,
        v = l && l.states,
        w = b.plotLeft,
        z;
      if (!d) {
        p = c.text(f.rangeSelectorZoom, w, b.plotTop - 10).css(s.labelStyle).add();
        z = w + p.getBBox().width + 5;
        bt(r, function (a, d) {
          q[d] = c.button(a.text, z, b.plotTop - 25, function () {
            u(d, a);
            this.isActive = true
          }, l, v && v.hover, v && v.select).css({
            textAlign: "center"
          }).add();
          z += q[d].width + (s.buttonSpacing || 0);
          if (o === d) {
            q[d].setState(2)
          }
        });
        if (t) {
          n = g = bT("div", null, {
            position: "relative",
            height: 0,
            fontFamily: i.fontFamily,
            fontSize: i.fontSize,
            zIndex: 1
          });
          e.parentNode.insertBefore(g, e);
          m = g = bT("div", null, bE({
            position: "absolute",
            top: b.plotTop - 25 + "px",
            right: b.chartWidth - b.plotLeft - b.plotWidth + "px"
          }, s.inputBoxStyle), g);
          j = y("min");
          k = y("max")
        }
      }
      if (t) {
        x(j, a);
        x(k, h)
      }
      d = true
    }

    function y(a) {
      var c = a === "min",
        d;
      l[a] = bT("span", {
        innerHTML: f[c ? "rangeSelectorFrom" : "rangeSelectorTo"]
      }, s.labelStyle, g);
      d = bT("input", {
        name: a,
        className: K + "range-selector",
        type: "text"
      }, bE({
        width: "80px",
        height: "16px",
        border: "1px solid silver",
        marginLeft: "5px",
        marginRight: c ? "5px" : "0",
        textAlign: "center"
      }, s.inputStyle), g);
      d.onfocus = d.onblur = function (a) {
        a = a || window.event;
        d.hasFocus = a.type === "focus";
        x(d)
      };
      d.onchange = function () {
        var a = d.value,
          e = Date.parse(a),
          f = b.xAxis[0].getExtremes();
        if (isNaN(e)) {
          e = a.split("-");
          e = Date.UTC(bG(e[0]), bG(e[1]) - 1, bG(e[2]))
        }
        if (!isNaN(e) && (c && e > f.dataMin && e < k.HCTime || !c && e < f.dataMax && e > j.HCTime)) {
          b.xAxis[0].setExtremes(c ? e : f.min, c ? f.max : e)
        }
      };
      return d
    }

    function x(a, b) {
      var c = a.hasFocus ? s.inputEditDateFormat || "%Y-%m-%d" : s.inputDateFormat || "%b %e, %Y";
      if (b) {
        a.HCTime = b
      }
      a.value = C(c, a.HCTime)
    }

    function w() {
      b.extraTopMargin = 25;
      s = b.options.rangeSelector;
      r = s.buttons || t;
      var c = s.selected;
      by(e, cY, v);
      if (c !== a && r[c]) {
        u(c, r[c], false)
      }
      by(b, "load", function () {
        by(b.xAxis[0], "afterSetExtremes", function () {
          if (q[o]) {
            q[o].setState(0)
          }
          o = null
        })
      })
    }

    function v() {
      if (j) {
        j.blur()
      }
      if (k) {
        k.blur()
      }
    }

    function u(a, c, d) {
      var e = b.xAxis[0],
        f = e && e.getExtremes(),
        g, j = f && f.dataMin,
        k = f && f.dataMax,
        l, m = e && i(f.max, k),
        n = new Date(m),
        p = c.type,
        r = c.count,
        s, t, u, v, w = {
          millisecond: 1,
          second: 1e3,
          minute: 60 * 1e3,
          hour: 3600 * 1e3,
          day: 24 * 3600 * 1e3,
          week: 7 * 24 * 3600 * 1e3
        };
      if (j === null || k === null || a === o) {
        return
      }
      if (w[p]) {
        t = w[p] * r;
        l = h(m - t, j);
        var x = r / 24 / 60;
        if (chartType == "day" || x > (new TimeSpan(k - j)).getDays() || (new Date(l)).add(-x).days() < j) {
          d = false;
          j = l;
          k = m;
          highExtreme = m;
          manualRedraw(x)
        } else {
          lowExtreme = l
        }
      } else if (p === "month") {
        n.setMonth(n.getMonth() - r);
        l = h(n.getTime(), j);
        t = 30 * 24 * 3600 * 1e3 * r;
        if (n.getTime() < l) {
          d = false;
          l = n.getTime();
          j = l;
          m = highExtreme;
          k = m;
          var y = (new TimeSpan(m - l)).getDays();
          manualRedraw(y)
        }
      } else if (p === "ytd") {
        n = new Date(0);
        g = new Date;
        v = g.getFullYear();
        n.setFullYear(v);
        if (String(v) !== C("%Y", n)) {
          n.setFullYear(v - 1)
        }
        l = u = h(j || 0, n.getTime());
        g = g.getTime();
        m = i(k || g, g)
      } else if (p === "year") {
        n.setFullYear(n.getFullYear() - r);
        l = h(j, n.getTime());
        t = 365 * 24 * 3600 * 1e3 * r;
        if (n.getTime() < l) {
          d = false;
          l = n.getTime();
          j = l;
          m = highExtreme;
          k = m;
          var y = (new TimeSpan(m - l)).getDays();
          manualRedraw(y)
        }
      } else if (p === "all" && e) {
        l = Number(new Date(minStartDate));
        j = l;
        m = Number(new Date(maxEndDate));
        k = m;
        highExtreme = m;
        lowExtreme = l;
        var y = (new TimeSpan(m - l)).getDays();
        d = false;
        manualRedraw(y)
      }
      if (q[a]) {
        q[a].setState(2)
      }
      if (!e) {
        s = b.options.xAxis;
        s[0] = bx(s[0], {
          range: t,
          min: u
        });
        o = a
      } else {
        setTimeout(function () {
          e.setExtremes(l, m, bR(d, 1), 0);
          o = a
        }, 1)
      }
    }
    var c = b.renderer,
      d, e = b.container,
      f = B.lang,
      g, j, k, l = {},
      m, n, o, p, q = [],
      r, s, t = [{
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
        type: "ytd",
        text: "YTD"
      }, {
        type: "year",
        count: 1,
        text: "1y"
      }, {
        type: "all",
        text: "All"
      }];
    b.resetZoomEnabled = false;
    w();
    return {
      render: z,
      destroy: A
    }
  };
  cy.prototype.callbacks.push(function (a) {
    function k() {
      if (c) {
        bz(a, "resize", e);
        bz(a.xAxis[0], "afterSetExtremes", g)
      }
      if (d) {
        bz(a, "resize", f);
        bz(a.xAxis[0], "afterSetExtremes", j)
      }
    }

    function j(a) {
      d.render(a.min, a.max)
    }

    function g(a) {
      c.render(a.min, a.max)
    }

    function f() {
      b = a.xAxis[0].getExtremes();
      d.render(b.min, b.max)
    }

    function e() {
      b = a.xAxis[0].getExtremes();
      c.render(h(b.min, b.dataMin), i(b.max, b.dataMax))
    }
    var b, c = a.scroller,
      d = a.rangeSelector;
    if (c) {
      by(a.xAxis[0], "afterSetExtremes", g);
      by(a, "resize", e);
      e()
    }
    if (d) {
      by(a.xAxis[0], "afterSetExtremes", j);
      by(a, "resize", f);
      f()
    }
    by(a, "destroy", k)
  });
  Highcharts.StockChart = function (a, b) {
    var c = a.series,
      d, e = {
        marker: {
          enabled: false,
          states: {
            hover: {
              enabled: true,
              radius: 5
            }
          }
        },
        gapSize: 5,
        shadow: false,
        states: {
          hover: {
            lineWidth: 2
          }
        },
        dataGrouping: {
          enabled: true
        }
      };
    a.xAxis = bw(bQ(a.xAxis || {}), function (a) {
      return bx({
        minPadding: 0,
        maxPadding: 0,
        ordinal: true,
        title: {
          text: null
        },
        showLastLabel: true
      }, a, {
          type: "datetime",
          categories: null
        })
    });
    a.yAxis = bw(bQ(a.yAxis || {}), function (a) {
      d = a.opposite;
      return bx({
        labels: {
          align: d ? "right" : "left",
          x: d ? -2 : 2,
          y: -2
        },
        showLastLabel: false,
        title: {
          text: null
        }
      }, a)
    });
    a.series = null;
    a = bx({
      chart: {
        panning: true
      },
      navigator: {
        enabled: true
      },
      scrollbar: {
        enabled: true
      },
      rangeSelector: {
        enabled: true
      },
      title: {
        text: null
      },
      tooltip: {
        shared: true,
        crosshairs: true
      },
      legend: {
        enabled: false
      },
      plotOptions: {
        line: e,
        spline: e,
        area: e,
        areaspline: e,
        column: {
          shadow: false,
          borderWidth: 0,
          dataGrouping: {
            enabled: true
          }
        }
      }
    }, a, {
        chart: {
          inverted: false
        }
      });
    a.series = c;
    return new cy(a, b)
  };
  var db = cL.init,
    dc = cL.processData,
    dd = cz.prototype.tooltipFormatter;
  cL.init = function () {
    db.apply(this, arguments);
    var a = this,
      b = a.options.compare;
    if (b) {
      a.modifyValue = function (a, c) {
        var d = this.compareValue;
        a = b === "value" ? a - d : a = 100 * (a / d) - 100;
        if (c) {
          c.change = a
        }
        return a
      }
    }
  };
  cL.processData = function () {
    var a = this;
    dc.apply(this);
    if (a.options.compare) {
      var b = 0,
        c = a.processedXData,
        d = a.processedYData,
        e = d.length,
        f = a.xAxis.getExtremes().min;
      for (; b < e; b++) {
        if (typeof d[b] === cQ && c[b] >= f) {
          a.compareValue = d[b];
          break
        }
      }
    }
  };
  cz.prototype.tooltipFormatter = function (a) {
    var b = this;
    a = a.replace("{point.change}", (b.change > 0 ? "+" : "") + bV(b.change, b.series.tooltipOptions.changeDecimals || 2));
    return dd.apply(this, [a])
  };
  (function () {
    var b = cL.init,
      c = cL.getSegments;
    cL.init = function () {
      var c = this,
        d, e;
      b.apply(c, arguments);
      d = c.chart;
      e = c.xAxis;
      if (e && e.options.ordinal) {
        by(c, "updatedData", function () {
          delete e.ordinalIndex
        })
      }
      if (e && e.options.ordinal && !e.hasOrdinalExtension) {
        e.hasOrdinalExtension = true;
        e.beforeSetTickPositions = function () {
          var b = this,
            c, d = [],
            e = false,
            f, g;
          if (b.options.ordinal) {
            bt(b.series, function (a, b) {
              if (a.visible !== false) {
                d = d.concat(a.processedXData);
                if (b) {
                  d.sort(function (a, b) {
                    return a - b
                  });
                  b = d.length - 1;
                  while (b--) {
                    if (d[b] === d[b + 1]) {
                      d.splice(b, 1)
                    }
                  }
                }
              }
            });
            c = d.length;
            if (c > 2) {
              f = d[1] - d[0];
              g = c - 1;
              while (g-- && !e) {
                if (d[g + 1] - d[g] !== f) {
                  e = true
                }
              }
            }
            if (e) {
              b.ordinalSlope = (d[c - 1] - d[0]) / (c - 1);
              b.ordinalOffset = d[0];
              b.ordinalPositions = d
            } else {
              b.ordinalPositions = b.ordinalSlope = b.ordinalOffset = a
            }
          }
        };
        e.val2lin = function (a, b) {
          var c = this,
            d = c.ordinalPositions;
          if (!d) {
            return a
          } else {
            var e = d.length,
              f, g, h;
            f = e;
            while (f--) {
              if (d[f] === a) {
                h = f;
                break
              }
            }
            f = e - 1;
            while (f--) {
              if (a > d[f]) {
                g = (a - d[f]) / (d[f + 1] - d[f]);
                h = f + g;
                break
              }
            }
            return b ? h : c.ordinalSlope * (h || 0) + c.ordinalOffset
          }
        };
        e.lin2val = function (b, c) {
          var d = this,
            e = d.ordinalPositions;
          if (!e) {
            return b
          } else {
            var g = d.ordinalSlope,
              h = d.ordinalOffset,
              i = e.length - 1,
              j, k, l;
            if (c) {
              if (b < 0) {
                b = e[0]
              } else if (b > i) {
                b = e[i]
              } else {
                i = f(b);
                l = b - i
              }
            } else {
              while (i--) {
                j = g * i + h;
                if (b >= j) {
                  k = g * (i + 1) + h;
                  l = (b - j) / (k - j);
                  break
                }
              }
            }
            return l !== a && e[i] !== a ? e[i] + l * (e[i + 1] - e[i]) : b
          }
        };
        e.getExtendedPositions = function () {
          var a = e.series[0].currentDataGrouping,
            b = e.ordinalIndex,
            c = a ? a.count + a.unitName : "raw",
            f = e.getExtremes(),
            g, h;
          if (!b) {
            b = e.ordinalIndex = {}
          }
          if (!b[c]) {
            g = {
              series: [],
              getExtremes: function () {
                return {
                  min: f.dataMin,
                  max: f.dataMax
                }
              },
              options: {
                ordinal: true
              }
            };
            bt(e.series, function (b) {
              h = {
                xAxis: g,
                xData: b.xData,
                chart: d
              };
              h.options = {
                dataGrouping: a ? {
                  enabled: true,
                  forced: true,
                  approximation: "open",
                  units: [
                    [a.unitName, [a.count]]
                  ]
                } : {
                    enabled: false
                  }
              };
              b.processData.apply(h);
              g.series.push(h)
            });
            e.beforeSetTickPositions.apply(g);
            b[c] = g.ordinalPositions
          }
          return b[c]
        };
        e.postProcessTickInterval = function (a) {
          var b = this.ordinalSlope;
          return b ? a / (b / e.closestPointRange) : a
        };
        by(e, "afterSetTickPositions", function (a) {
          var b = e.options,
            c = b.tickPixelInterval,
            d = a.tickPositions;
          if (e.ordinalPositions && bO(c)) {
            var f = d.length,
              g, h, i = d.info,
              j = i ? i.higherRanks : [];
            while (f--) {
              g = e.translate(d[f]);
              if (h && h - g < c * .6) {
                d.splice(j[d[f]] && !j[d[f + 1]] ? f + 1 : f, 1)
              } else {
                h = g
              }
            }
          }
        });
        var g = d.pan;
        d.pan = function (a) {
          var b = d.xAxis[0],
            c = false;
          if (b.options.ordinal) {
            var e = d.mouseDownX,
              f = b.getExtremes(),
              k = f.dataMax,
              l = f.min,
              m = f.max,
              n, o, p = d.hoverPoints,
              q = b.closestPointRange,
              r = b.translationSlope * (b.ordinalSlope || q),
              s = (e - a) / r,
              t = {
                ordinalPositions: b.getExtendedPositions()
              },
              u, v, w = b.lin2val,
              x = b.val2lin,
              y;
            if (!t.ordinalPositions) {
              c = true
            } else if (j(s) > 1) {
              if (p) {
                bt(p, function (a) {
                  a.setState()
                })
              }
              if (s < 0) {
                v = t;
                y = b.ordinalPositions ? b : t
              } else {
                v = b.ordinalPositions ? b : t;
                y = t
              }
              u = y.ordinalPositions;
              if (k > u[u.length - 1]) {
                u.push(k)
              }
              n = w.apply(v, [x.apply(v, [l, true]) + s, true]);
              o = w.apply(y, [x.apply(y, [m, true]) + s, true]);
              if (n > i(f.dataMin, l) && o < h(k, m)) {
                b.setExtremes(n, o, true, false)
              }
              d.mouseDownX = a;
              bS(d.container, {
                cursor: "move"
              })
            }
          } else {
            c = true
          }
          if (c) {
            g.apply(d, arguments)
          }
        }
      }
    };
    cL.getSegments = function () {
      var a = this,
        b, d = a.options.gapSize;
      c.apply(a);
      if (a.xAxis.options.ordinal && d) {
        b = a.segments;
        bt(b, function (c, e) {
          var f = c.length - 1;
          while (f--) {
            if (c[f + 1].x - c[f].x > a.xAxis.closestPointRange * d) {
              b.splice(e + 1, 0, c.splice(f + 1, c.length - f))
            }
          }
        })
      }
    }
  })();
  bE(Highcharts, {
    Chart: cy,
    dateFormat: C,
    pathAnim: E,
    getOptions: ci,
    hasRtlBug: v,
    numberFormat: bV,
    Point: cz,
    Color: ct,
    Renderer: w,
    seriesTypes: bD,
    setOptions: ch,
    Series: cA,
    addEvent: by,
    removeEvent: bz,
    createElement: bT,
    discardElement: cj,
    css: bS,
    each: bt,
    extend: bE,
    map: bw,
    merge: bx,
    pick: bR,
    splat: bQ,
    extendClass: bU,
    product: "Highstock",
    version: "1.1.2"
  })
})()
