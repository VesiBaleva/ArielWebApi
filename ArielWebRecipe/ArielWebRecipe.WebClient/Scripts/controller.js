/// <reference path="class.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function() {
    var rootUrl = "http://arialwebapirecipe.apphb.com/api/";

    var updateTimer = null;

    var Controller = Class.create({
        init: function () {
            //set on persister
         //   this.persister = persisters.get(rootUrl);
        },

        loadUI: function (selector) {
            //set on persister
            //if (this.persister.isUserLoggedIn()) {
                this.loadMainUI(selector);
           //}
           //else {
           //    this.loadLoginFormUI(selector);
           //}
           //this.attachUIEventHandlers(selector);
        },

        loadLoginFormUI: function (selector) {
            var loginFormHtml = ui.loginForm()
            $(selector).html(loginFormHtml);
        },

        loadMainUI: function (selector) {
            var self = this;
            var mainUIHtml =
				ui.mainUI("Vesi");                      //this.persister.nickname());
            $(selector).html(mainUIHtml);

            this.updateUI(selector);

            updateTimer = setInterval(function () {
                self.updateUI(selector);
            }, 80000);
        },

        updateUI: function (selector) {
            var recipeExample = {
                id: "",
                title: "Title",
                pictureLink: "img/images.jpg"
            }

            var recipe_list = [];

            for (var i = 0; i < 9; i++) {
                recipeExample.id = i + 1;
                recipeExample.title += i;
                recipe_list.push(recipeExample);
            }

            var list = ui.recipesList(recipe_list);

            $(selector + " #recipes-list")
                .html(list);
            var userOperationUIHtml =
                ui.userOperationUI("Vesi");
            $(selector + " #user-operation").html(userOperationUIHtml);

            //this.persister.recipesAll(function (recipes) {
                

            //    var list = ui.recipesList(recipe_list);
            //    $(selector + " #recipes-list")
			//		.html(recipe_list);
            //    var userOperationUIHtml =
            //        ui.userOperationUI(this.persister.nickname());
            //    $(selector + " #user-operation").html(userOperationUIHtml);
            //});            
        }
    });
    return {
        get: function () {
            return new Controller();
        }
    }

}());

$(function () {
    var controller = controllers.get();
    controller.loadUI("#container");
})