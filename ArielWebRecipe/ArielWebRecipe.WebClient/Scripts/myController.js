﻿/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function () {

    function loadGameUI(selector) {
        if (!selector) {
            selector = defaultSelector;
        }
        console.log("Loading Game UI" + selector);
        var self = this;
        var gameUIHtml = "<h1>Logged in!!!</h1><button id='newRequest'>Get</button>" +
                        "<br />" +
                        "<div id='requestContent'></div>" +
                         "<br />" +
                         "<button id='btn-logout'>Log Out</button>";

        $(selector).html(gameUIHtml);
    }

    function loadLoginFormUI(selector) {
        console.log("Loading LogIN");
        if (!selector) {
            selector = defaultSelector;
        }
        var loginFormHtml = ui.loginForm()
        $(selector).html(loginFormHtml);
    }

    var updateTimer = null;

    var rootUrl = "http://arialwebapirecipe.apphb.com/api/";
    var defaultSelector = null;

    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
        },
        loadUI: function (selector) {
            defaultSelector = selector;

            if (this.persister.isUserLoggedIn()) {
                this.persister.user.checkSessionKey(function () {
                    loadGameUI();
                }, function () {
                    loadLoginFormUI(selector);
                });
            }
            else {
                this.loadLoginFormUI(selector);
            }
            this.attachUIEventHandlers(selector);

            //this.loadBaseUI(selector);
        },

        loadMainUI: function (selector) {
            var self = this;
            var mainUIHtml =
				ui.mainUI("Vesi");                      //this.persister.nickname());
            $(selector).html(mainUIHtml);

            this.updateUI(selector);

            updateTimer = setInterval(function () {
                // self.updateUI(selector);
            }, 15000);
        },

        loadLoginFormUI: function (selector) {
            loadLoginFormUI(selector);
        },

        loadGameUI: function (selector) {
            loadGameUI(selector);
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            //Basic request
            wrapper.on("click", "#newRequest", function () {
                //console.log("Even on newRequest");

                //self.persister.imageUpload.upload()
                var newStep = {
                    Description : "Add an egg",
                    Order : 1,
                    PreparationTime: 3
                }

                var newStep2 = {
                    Description : "Add an egg",
                    Order : 2,
                    PreparationTime: 3
                }

                var bake = {
                    Description : "Bake",
                    Order : 3,
                    PreparationTime: 5
                }

                var steps = [];
                steps.push(newStep);
                steps.push(newStep2);
                steps.push(bake);

                var newRecipe = {
                    Title : "newRecipe",
                    PreparationSteps: steps,
                }

                

                //self.persister.recipe.create(newRecipe, function (data) {
                //    //wrapper.find("#error-messages").text(data.responseJSON.Message);
                //}, function (err) {
                //    //wrapper.find("#error-messages").text(err.responseJSON.Message);
                //});

                //Check input Files
                var recipeImage = $('#file')[0].files[0];
                var recipeImageName = $('#file')[0].value;
                var recipeImageExtension = recipeImageName.substring(recipeImageName.length - 4);

                var fd = new FormData();
                fd.append(recipeImageName, recipeImage);
                //fd.append("Recipe", JSON.stringify(newRecipe));
                fd.append("SessionKey", "SomeSessionKey");
                fd.append("ImageExtension", recipeImageExtension);
                fd.append("RecipeId", 8);

                self.persister.imageUpload.uploadRawAjax(fd);

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