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

            if (this.persister.isUserLoggedIn()) {
                this.loadGameUI(selector);
            }
            else {
                this.loadLoginFormUI(selector);
            }
            this.attachUIEventHandlers(selector);

            //this.loadBaseUI(selector);
        },

        loadLoginFormUI: function (selector) {
            var loginFormHtml = ui.loginForm()
            $(selector).html(loginFormHtml);
        },

        loadGameUI: function (selector) {
            var self = this;
            var gameUIHtml = "<h1>Logged in!!!</h1><button id='newRequest'>Get</button>" +
                            "<br />" +
                            "<div id='requestContent'></div>" +
             "<br />" +
             "<button id='btn-logout'>Log Out</button>";
				
            $(selector).html(gameUIHtml);
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            //Basic request
            wrapper.on("click", "#newRequest", function () {
                console.log("Even on newRequest");

                var fd = new FormData();
                fd.append("fileToUpload", $('#file')[0].files[0]);
                fd.append("myName", "BeboBeboBe");

                $.ajax({
                    url: "http://arialwebapirecipe.apphb.com/api/Users/testUpload",
                    type: "POST",
                    data: fd,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        // .. do something
                    },
                    error: function (jqXHR, textStatus, errorMessage) {
                        console.log(errorMessage); // Optional
                    }
                });
                

                //self.persister.user.test(function (data) {
                //    wrapper.find("#requestContent").html(JSON.stringify(data));
                //},

				//function (err) {
				//    alert(JSON.stringify(err));
				//});

                //return false;
            });
            
            //User login/logout handlers
            wrapper.on("click", "#btn-show-login", function () {
                wrapper.find(".button.selected").removeClass("selected");
                $(this).addClass("selected");
                wrapper.find("#login-form").show();
                wrapper.find("#register-form").hide();
            });
            wrapper.on("click", "#btn-show-register", function () {
                wrapper.find(".button.selected").removeClass("selected");
                $(this).addClass("selected");
                wrapper.find("#register-form").show();
                wrapper.find("#login-form").hide();
            });

            wrapper.on("click", "#btn-login", function () {
                var user = {
                    username: $(selector + " #tb-login-username").val(),
                    password: $(selector + " #tb-login-password").val()
                }

                self.persister.user.login(user, function () {
                    self.loadGameUI(selector);
                }, function (err) {
                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                });
                return false;
            });
            wrapper.on("click", "#btn-register", function () {
                var user = {
                    username: $(selector).find("#tb-register-username").val(),
                    nickname: $(selector).find("#tb-register-nickname").val(),
                    password: $(selector + " #tb-register-password").val()
                }
                self.persister.user.register(user, function () {
                    self.loadGameUI(selector);
                }, function (err) {
                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                });
                return false;
            });
            wrapper.on("click", "#btn-logout", function () {
                self.persister.user.logout(function () {
                    self.loadLoginFormUI(selector);
                    //clearInterval(updateTimer);
                }, function (err) {
                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                });
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