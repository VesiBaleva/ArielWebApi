/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function () {

    var updateTimer = null;

    var rootUrl = "http://arialwebapirecipe.apphb.com/api/";
    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
        },
        loadUI: function (selector) {

            //this.loadBaseUI(selector);

            this.attachUIEventHandlers(selector);
        },

        loadGameUI: function (selector) {
            var self = this;
            var gameUIHtml = "<button id='get'>Get<button>"
				
            $(selector).html(gameUIHtml);
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            //Basic request
            wrapper.on("click", "#newRequest", function () {
                console.log("Even on newRequest");

                self.persister.user.test(function (data) {
                    wrapper.find("#requestContent").html(JSON.stringify(data));
                },

				function (err) {
				    alert(JSON.stringify(err));
				});

                return false;
            });
            
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
    controller.loadUI("#wrapper");
});