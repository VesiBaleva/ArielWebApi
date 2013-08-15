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
		    '<span id="user-nickname">Welcome, ' +
				nickname +
		    '</span>' +
		    '<button id="btn-logout">Logout</button><br/>' +
            '<h2>A list of the recipes</h2>' +
            '<div class="row">' +                
                '<div class="span9">' +
                '<div id="recipes-list"></div>' +
                '</div>' +
                '<div class="span3">' +
                '<div id="user-operation"></div>' +
                '</div>' +
            '</div>';
        return html;
    }

    function buildRecipesList(recipes) {
        var list = '<div class="row-fluid">';
        
        for (var i = 0; i < recipes.length; i++) {

            var recipe = recipes[i];
            list +=
				'<div class="span4" data-recipe-id="' + recipe.id + '">' +
					'<a href="#" >' + $("<div> /").html(recipe.title).text() + '</a>' +
                    '<img src="' + $("<div />").html(recipe.pictureLink).text() + '" class="img-polaroid" />' +
				'</div>';
        }
        list += "</div>";
        return list;
    }

    function buildUserOperationUI(nickname) {
        var html =
		    '<form class="form-search">' +
                  '<div>' +
                      '<input type="text" class="input-medium search-query" />' +
                      '<button type="submit" class="btn">Search</button>' +
                  '</div>' +
            '</form>' +
            '<div>' +
                '<button class="btn btn-large btn-block" type="button">Create recipe</button>' +
            '</div>';
        return html;
    }


    return {
        loginForm:buildLoginForm,
        mainUI:buildMainUI,
        recipesList:buildRecipesList,
        userOperationUI:buildUserOperationUI
    }

}());