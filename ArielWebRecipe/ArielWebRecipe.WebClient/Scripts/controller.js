/// <reference path="class.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />
/// <reference path="myPersister.js" />

var controllers = (function() {
    var rootUrl = "http://localhost:9181/api/";

    var newRecipe;

    var updateTimer = null;

    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
        },

        loadUI: function (selector) {
            self = this;
            if (this.persister.isUserLoggedIn()) {
                this.persister.user.checkSessionKey(function () {
                    //loadGameUI();
                    self.loadMainUI(selector);
                }, function () {
                    self.loadLoginFormUI(selector);
                });
            }
            else {
                this.loadLoginFormUI(selector);
            }

            this.attachUIEventHandlers(selector);

            //defaultSelector = selector;

        },

        loadLoginFormUI: function (selector) {
            var loginFormHtml = ui.loginForm()
            $(selector).html(loginFormHtml);
        },

        loadMainUI: function (selector) {
            var self = this;
            var mainUIHtml =
				ui.mainUI(this.persister.user.getNickname());                      //this.persister.nickname());
            $(selector).html(mainUIHtml);

            this.updateUI(selector);
        },

        updateUI: function (selector) {
            var self = this;
            this.persister.recipe.page(0, function (data) {
                var list = ui.recipesList(data);
                console.log(JSON.stringify(data));
                $(selector + " #recipes-list")
                    .html(list);
                var userOperationUIHtml =
                    ui.userOperationUI(self.persister.user.getNickname());
                $(selector + " #user-operation").html(userOperationUIHtml);
            }, function (err) {
                alert(JSON.stringify(err));
            });

            
        },

        

        loadCreateRecipeUI: function (selector) {
            var self = this;
            var createRecipeUIHtml =
				ui.createRecipe("Vesi");                      //this.persister.nickname());
            $(selector).html(createRecipeUIHtml);
            newRecipe = {
                Title: "",
                PreparationSteps: []
            };
        },

        loadRecipeDetailsUI: function (selector, data) {

   
            var self = this;
            var recipeDetailsUIHtml =
				ui.recipeDetailsUI(data);                      //this.persister.nickname());
            $(selector).html(recipeDetailsUIHtml);

        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

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
                    Description: $(selector + " #tb-description").val(),
                    PreparationTime: $(selector + " #tb-preparation-time").val()
                }

                newRecipe.PreparationSteps.push(step);
                step.Order = newRecipe.PreparationSteps.length;

                var html = '<br /><div class="row"> ' +
                    '<div class="span2 label">' +
                    'Step: </div><div class="span4">' + step.Description +
                    '<br />' +
                    'Preparation time: ' + step.PreparationTime
                    '</div>';
                $("#preparation-steps").append(html);
                console.log("save");
                wrapper.find("#panel-step").hide();
                
            });

            wrapper.on("click", "#btn-recipe", function () {

                var recipe = {
                    id: $(this).parent().parent().data("recipe-id")
                };

                self.persister.recipe.open((recipe.id)*1, function (data) {
                    wrapper.find("#recipe-holder-form").hide();
                    self.loadRecipeDetailsUI("#recipeDetails-holder", data);
                }, function (err) {
                    alert(JSON.stringify(err));
                });

                
            });

            wrapper.on("click", "#btn-close-step", function () {
                wrapper.find("#panel-step").hide();

            });

            //Create Recipe
            wrapper.on("click", "#btn-save-recipe", function () {
                newRecipe.Title = $(selector + " #recipe-title").val()

                newRecipe.InputImageId = "fileRecipe";

                self.persister.recipe.create(newRecipe, function (data) {
                    var imagelink = "#" + newRecipe.InputImageId;

                    var recipeImage = $(imagelink)[0].files[0];
                    var recipeImageName = $(imagelink)[0].value;
                    var recipeImageExtension = recipeImageName.substring(recipeImageName.length - 4);

                    var fd = new FormData();
                    fd.append(recipeImageName, recipeImage);
                    //fd.append("Recipe", JSON.stringify(newRecipe));
                    fd.append("SessionKey", self.persister.user.getSessionKey());
                    fd.append("ImageExtension", recipeImageExtension);
                    fd.append("RecipeId", data.Id);

                    self.persister.imageUpload.uploadRawAjax(fd);

                    self.loadRecipeDetailsUI("#recipeDetails-holder", newRecipe);
                });
            });

            //Search
            wrapper.on("click", "#btn-search", function () {

                var queryString = wrapper.find("#input-field").val();
                console.log("Searching for " + queryString);
                if (queryString) {
                    self.persister.recipe.search(queryString, function (data) {
                        var list = ui.recipesList(data);

                        $(selector + " #recipes-list")
                            .html(list);
                        var userOperationUIHtml =
                            ui.userOperationUI(self.persister.user.getNickname());
                        $(selector + " #user-operation").html(userOperationUIHtml);
                    }, function (err) {
                        alert(err)
                    });
                }
            });


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
                    self.loadMainUI(selector);
                }, function (err) {
                    //wrapper.find("#error-messages").text(err.responseJSON.Message);
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
                    self.loadMainUI(selector);
                }, function (err) {
                    alert(err.responseJSON.Message);
                });
                return false;
            });
            wrapper.on("click", "#btn-logout", function () {
                self.persister.user.logout(function () {
                    self.loadLoginFormUI(selector);
                    //clearInterval(updateTimer);
                }, function (err) {
                    alert(err.responseJSON.Message);
                });
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