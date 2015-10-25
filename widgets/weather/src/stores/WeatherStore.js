var AppFlowController = require('../controller/AppFlowController');
var Constants = require('../constants/Constants');

var Promise = require('es6-promise').Promise;
var request = require('superagent');
var _ = require('underscore');


var ForecastData;
var TwoWeeksData;
var SunMoonData;


var WeatherStore = {
    getForecastData: function() {
        return ForecastData;
    },

    getTwoWeeksData: function() {
        return TwoWeeksData;
    },

    getSunMoonData: function() {
        return SunMoonData;
    },

    callbackForecastData: AppFlowController.addTarget(Constants.FlowID.GET_FORECAST_DATA, function(payload) {
        return new Promise(function(resolve, reject){
            request
                .get(Constants.API.GET_FORECAST_DATA + "/" + Constants.CountryCode.Seoul)
                .query({client_id: Constants.API.CLIENT_ID, client_secret: Constants.API.CLIENT_SECRET, limit: payload.dataAmount, filter: '3hr'})
                .end(function(err,res) {
                    if (err) { console.log(err); }
                    else {
                        ForecastData = res.body.response[0];
                        console.log("ForecastData", res.body.response[0]);
                        resolve();
                    }
                });
        });
    }),

    callbackTwoWeeksData: AppFlowController.addTarget(Constants.FlowID.GET_14_FORECAST_DATA, function(payload) {
        return new Promise(function(resolve, reject) {
            request
                .get(Constants.API.GET_FORECAST_DATA + "/" + Constants.CountryCode.Seoul)
                .query({client_id: Constants.API.CLIENT_ID, client_secret: Constants.API.CLIENT_SECRET, limit: 14})
                .end(function(err,res) {
                    if (err) { console.log(err); }
                    else {
                        TwoWeeksData = res.body.response[0];
                        console.log("14Day", res.body.response[0]);
                        resolve();
                    }
                });
        });
    }),

    callbackSunMoonData: AppFlowController.addTarget(Constants.FlowID.GET_SUN_MOON_DATA, function(payload) {
        return new Promise(function(resolve, reject){
            request
                .get(Constants.API.GET_SUN_MOON_DATA + "/" + Constants.CountryCode.Seoul)
                .query({client_id: Constants.API.CLIENT_ID, client_secret: Constants.API.CLIENT_SECRET, from: "today", to: "+2week", limit: 14})
                .end(function(err,res) {
                    if (err) { console.log(err); }
                    else {
                        SunMoonData = res.body.response;
                        console.log("SunMoonData", res.body.response);
                        resolve();
                    }
                });
        });
    }),


    callbackActiveApp: AppFlowController.addTarget(
        Constants.FlowID.ACTIVE_APP,
        function() {
            return new Promise(function(resolve, reject) {
                resolve();
            });
        }
    ),

    callbackDisableApp: AppFlowController.addTarget(
        Constants.FlowID.DISABLE_APP,
        function() {
            return new Promise(function(resolve, reject) {
                resolve();
            });
        }
    )
};

module.exports = WeatherStore;