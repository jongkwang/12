!function e(i,s,n){function t(r,d){if(!s[r]){if(!i[r]){var o="function"==typeof require&&require;if(!d&&o)return o(r,!0);if(a)return a(r,!0);var l=new Error("Cannot find module '"+r+"'");throw l.code="MODULE_NOT_FOUND",l}var c=s[r]={exports:{}};i[r][0].call(c.exports,function(e){var s=i[r][1][e];return t(s?s:e)},c,c.exports,e,i,s,n)}return s[r].exports}for(var a="function"==typeof require&&require,r=0;r<n.length;r++)t(n[r]);return t}({1:[function(e,s,n){$(document).ready(function(e){function s(){n(e(".cd-headline.letters").find("b")),t(e(".cd-headline"))}function n(s){s.each(function(){var s=e(this),n=s.text().split(""),t=s.hasClass("is-visible");for(i in n)s.parents(".rotate-2").length>0&&(n[i]="<em>"+n[i]+"</em>"),n[i]=t?'<i class="in">'+n[i]+"</i>":"<i>"+n[i]+"</i>";var a=n.join("");s.html(a).css("opacity",1)})}function t(i){var s=u;i.each(function(){var i=e(this);if(i.hasClass("loading-bar"))s=f,setTimeout(function(){i.find(".cd-words-wrapper").addClass("is-loading")},h);else if(i.hasClass("clip")){var n=i.find(".cd-words-wrapper"),t=n.width()+10;n.css("width",t)}else if(!i.hasClass("type")){var r=i.find(".cd-words-wrapper b"),d=0;r.each(function(){var i=e(this).width();i>d&&(d=i)}),i.find(".cd-words-wrapper").css("width",d)}setTimeout(function(){a(i.find(".is-visible").eq(0))},s)})}function a(e){var i=l(e);if(e.parents(".cd-headline").hasClass("type")){var s=e.parent(".cd-words-wrapper");s.addClass("selected").removeClass("waiting"),setTimeout(function(){s.removeClass("selected"),e.removeClass("is-visible").addClass("is-hidden").children("i").removeClass("in").addClass("out")},v),setTimeout(function(){r(i,C)},m)}else if(e.parents(".cd-headline").hasClass("letters")){var n=e.children("i").length>=i.children("i").length?!0:!1;d(e.find("i").eq(0),e,n,p),o(i.find("i").eq(0),i,n,p)}else e.parents(".cd-headline").hasClass("clip")?e.parents(".cd-words-wrapper").animate({width:"2px"},w,function(){c(e,i),r(i)}):e.parents(".cd-headline").hasClass("loading-bar")?(e.parents(".cd-words-wrapper").removeClass("is-loading"),c(e,i),setTimeout(function(){a(i)},f),setTimeout(function(){e.parents(".cd-words-wrapper").addClass("is-loading")},h)):(c(e,i),setTimeout(function(){a(i)},u))}function r(e,i){e.parents(".cd-headline").hasClass("type")?(o(e.find("i").eq(0),e,!1,i),e.addClass("is-visible").removeClass("is-hidden")):e.parents(".cd-headline").hasClass("clip")&&e.parents(".cd-words-wrapper").animate({width:e.width()+10},w,function(){setTimeout(function(){a(e)},T)})}function d(i,s,n,t){if(i.removeClass("in").addClass("out"),i.is(":last-child")?n&&setTimeout(function(){a(l(s))},u):setTimeout(function(){d(i.next(),s,n,t)},t),i.is(":last-child")&&e("html").hasClass("no-csstransitions")){var r=l(s);c(s,r)}}function o(e,i,s,n){e.addClass("in").removeClass("out"),e.is(":last-child")?(i.parents(".cd-headline").hasClass("type")&&setTimeout(function(){i.parents(".cd-words-wrapper").addClass("waiting")},200),s||setTimeout(function(){a(i)},u)):setTimeout(function(){o(e.next(),i,s,n)},n)}function l(e){return e.is(":last-child")?e.parent().children().eq(0):e.next()}function c(e,i){e.removeClass("is-visible").addClass("is-hidden"),i.removeClass("is-hidden").addClass("is-visible")}var u=2500,f=3800,h=f-3e3,p=50,C=150,v=500,m=v+800,w=600,T=1500;s()})},{}]},{},[1]);