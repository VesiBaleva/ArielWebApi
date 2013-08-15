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
           this.attachUIEventHandlers(selector);
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
               // self.updateUI(selector);
            }, 15000);
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
        },

        

        loadCreateRecipeUI: function (selector) {
            var self = this;
            var createRecipeUIHtml =
				ui.createRecipe("Vesi");                      //this.persister.nickname());
            $(selector).html(createRecipeUIHtml);
                        
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

          // wrapper.on("click", "#btn-show-login", function () {
          //     wrapper.find(".button.selected").removeClass("selected");
          //     $(this).addClass("selected");
          //     wrapper.find("#login-form").show();
          //     wrapper.find("#register-form").hide();
          // });
          //
          // wrapper.on("click", "#btn-show-register", function () {
          //     wrapper.find(".button.selected").removeClass("selected");
          //     $(this).addClass("selected");
          //     wrapper.find("#register-form").show();
          //     wrapper.find("#login-form").hide();
          // });
          //
          // wrapper.on("click", "#btn-login", function () {
          //     var user = {
          //         username: $(selector + " #tb-login-username").val(),
          //         password: $(selector + " #tb-login-password").val()
          //     }
          //
          //     self.persister.user.login(user, function () {
          //         self.loadGameUI(selector);
          //     }, function (err) {
          //         wrapper.find("#error-messages").text(err.responseJSON.Message);
          //     });
          //     return false;
          // });
          // wrapper.on("click", "#btn-register", function () {
          //     var user = {
          //         username: $(selector).find("#tb-register-username").val(),
          //         nickname: $(selector).find("#tb-register-nickname").val(),
          //         password: $(selector + " #tb-register-password").val()
          //     }
          //     self.persister.user.register(user, function () {
          //         self.loadGameUI(selector);
          //     }, function (err) {
          //         wrapper.find("#error-messages").text(err.responseJSON.Message);
          //     });
          //     return false;
          // });
          // wrapper.on("click", "#btn-logout", function () {
          //     self.persister.user.logout(function () {
          //         self.loadLoginFormUI(selector);
          //         clearInterval(updateTimer);
          //     }, function (err) {
          //     });
          // });
            wrapper.on("click", "#btn-create-recipe", function () {
              //  wrapper.find(".button.selected").removeClass("selected");
                //  $(this).addClass("selected");
                console.log("create");
                wrapper.find("#recipe-holder-form").hide();
                self.loadCreateRecipeUI("#recipe-create-form");
            });
            

            wrapper.on("click", "#btn-upload-picture", function () {
                
            });

            wrapper.on("click", "#btn-step", function () {
                //  wrapper.find(".button.selected").removeClass("selected");
                //  $(this).addClass("selected");
                wrapper.find("#panel-step").show();
            });

            wrapper.on("click", "#btn-save-step", function () {
                
                var step = {
                    stepDescription: $(selector + " #tb-description").val(),
                    stepPreparationTime: $(selector + " #tb-preparation-time").val()
                }
                $(selector + " #tb-description").val("");
                stepPreparationTime: $(selector + " #tb-preparation-time").val("");
                username: $(selector + " #tb-login-username").val()
                var html = '<br /><div class="row"> ' +
                    '<div class="span2 label">' +
                    'Step: </div><div class="span4">' + step.stepDescription +
                    '<br />' +
                    'Preparation time: ' + step.stepPreparationTime
                    '</div>';
                $("#preparation-steps").append(html);
                console.log("save");
                wrapper.find("#panel-step").hide();
                
            });

            wrapper.on("click", "#btn-close-step", function () {
                wrapper.find("#panel-step").hide();
            });
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
    controller.loadUI(".container");
})