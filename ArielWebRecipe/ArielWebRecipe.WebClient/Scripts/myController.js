/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function () {

    var updateTimer = null;

    var rootUrl = "http://arialwebapirecipe.apphb.com/api";
    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
        },
        loadUI: function (selector) {

            this.loadBaseUI(selector);

            this.attachUIEventHandlers(selector);
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            
        },
    });
    return {
        get: function () {
            return new Controller();
        }
    }
}());

$(function () {
    var controller = controllers.get();
    controller.loadUI("#content");
});