var ui = (function () {

    function buildLoginForm() {
        var html = 
            '<div id="login-form-holder">' +
				'<form>' +
					'<div id="login-form">' +
						'<label for="tb-login-username">Username: </label>' +
						'<input type="text" id="tb-login-username"><br />' +
						'<label for="tb-login-password">Password: </label>' +
						'<input type="text" id="tb-login-password"><br />' +
						'<button id="btn-login" class="button">Login</button>' +
					'</div>' +
					'<div id="register-form" style="display: none">' +
						'<label for="tb-register-username">Username: </label>' +
						'<input type="text" id="tb-register-username"><br />' +
						'<label for="tb-register-nickname">Nickname: </label>' +
						'<input type="text" id="tb-register-nickname"><br />' +
						'<label for="tb-register-password">Password: </label>' +
						'<input type="text" id="tb-register-password"><br />' +
						'<button id="btn-register" class="button">Register</button>' +
					'</div>' +
					'<a href="#" id="btn-show-login" class="button selected">Login</a>' +
					'<a href="#" id="btn-show-register" class="button">Register</a>' +
				'</form>' +
				'<div id="error-messages"></div>' +
            '</div>';
        return html;
    }

    function buildMainUI(nickname) {
        var html = 
		    '<p class="lead" id="user-nickname">Welcome, ' +
				nickname +
		    '&nbsp;</p>' +
		    '<button class="btn" id="btn-logout">Logout</button><br/>' +
            '<div id=recipe-holder-form>' +
            '<h2>A list of the recipes</h2>' +
            '<div class="row">' +                
                '<div class="span9">' +
                '<div id="recipes-list"></div>' +
                '</div>' +
                '<div class="span3">' +
                '<div id="user-operation"></div>' +
                '</div>' +
            '</div>' +
            '</div>'+
            '<div id="recipe-create-form"></div>';
        return html;
    }

    function buildRecipesList(recipes) {
        var list = '';
        for (var i = 0; i < recipes.length/3; i++) {
            list += '<div class="row-fluid">';
            for (var j = 0; j < recipes.length / 3; j++) {
                var recipe = recipes[i,j];
                list +=
                    '<div class="span4" data-recipe-id="' + recipe.id + '">' +
                        '<h3><a href="#" >' + $("<div> /").html(recipe.title).text() + '</a></h3>' +
                        '<img src="' + $("<div />").html(recipe.pictureLink).text() + '" class="img-polaroid" />' +
                    '</div>';
            }
            list += '</div>';
        }
        list +=
            '<div class="row-fluid">' +
                    '<ul class="pager">' +
                      '<li class="previous disabled"><a href="#">Previous</a></li>' +
                      '<li class="next"><a href="#">Next</a></li>' +
                    '</ul>' +
            '</div>';
        return list;
    }

    function buildUserOperationUI(nickname) {
        var html =
		    '<form class="form-search">' +
                  '<div>' +
                      '<input type="text" class="input-medium search-query" />' +
                      '<button  class="btn">Search</button>' +
                  '</div>' +
            '</form>' +
            '<div>' +
                '<button id="btn-create-recipe" class="btn btn-large btn-block" type="button">Create recipe</button>' +
            '</div>';
        return html;
    }

    function buildCreateRecipeUI(nickname) {
        var html =

            '<h2>Create a new recipe</h2>' +
        
        '<form>' +
          '<fieldset>' +
            '<legend>Type name</legend>' +
            '<input type="text" placeholder="Name of the recipe" />' +
            '<br />' +
            '<button  class="btn" id="btn-upload-picture">Upload picture</button>' +
            '<legend>Preparation steps</legend>' +
                       
          '</fieldset>' +
        '</form>' +
        '<button id="btn-step" class="btn">Add Step</button> ' +
        '<div id="panel-step">' +
                '<label>Description</label>' +
                '<input type="text" placeholder="description step..." id="tb-description" />' +
                '<label>Preparation time</label>' +
                '<input type="text" placeholder="time..." id="tb-preparation-time" />' +
                '<br />' +
                '<button class="btn">Add Picture</button>' +
                '<br />' +
                '<button class="btn" id="btn-save-step">Save</button>' +
                '<button class="btn" id="btn-close-step">Close</button>' +
            '</div>' +
        '<div id="preparation-steps">' +
        '</div>';

        return html;
    }

    return {
        loginForm:buildLoginForm,
        mainUI:buildMainUI,
        recipesList:buildRecipesList,
        userOperationUI: buildUserOperationUI,
        createRecipe:buildCreateRecipeUI
    }

}());