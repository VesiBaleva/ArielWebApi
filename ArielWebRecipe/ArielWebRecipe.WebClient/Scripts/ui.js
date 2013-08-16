var ui = (function () {

    function buildLoginForm() {
        var html = 
             '<div class="span4">' +
      '<form class="form-signin">'+
   '     <h2 class="form-signin-heading">Please sign in</h2>'+
   '     <input type="text" class="input-block-level" placeholder="Email address">'+
   '     <input type="password" class="input-block-level" placeholder="Password">'+
   '     <button class="btn btn-large btn-primary" type="submit">Sign in</button>'+
   '   </form>'+
   ' </div>'+
   ' </div>';
        return html;
    }

    function buildRegisterForm() {
        var html =
             '<div class="span4">' +
      '<form class="form-signin">' +
   '     <h2 class="form-signin-heading">Please register</h2>' +
   '     <input type="text" class="input-block-level" placeholder="Email address">' +
   '     <input type="text" class="input-block-level" placeholder="Nickname">' +
   '     <input type="password" class="input-block-level" placeholder="Password">' +
   '     <button class="btn btn-large btn-primary" type="submit">Register</button>' +
   '   </form>' +
   ' </div>' +
   ' </div>';
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
            '<div id="recipe-create-form"></div>' +
            '<div id="recipeDetails-holder"></div>';
        return html;
    }

    function buildRecipesList(recipes) {
        if (recipes.length == 0) {
            return "<div>Nothing Found</div>";
        }
        var list = '';
        var number = 0;
            for (var i = 0; i < 3; i++) {
                list += '<div class="row-fluid">';
                for (var j = 0; j < 3; j++) {
                    var recipe = recipes[number];
                    if (!recipe.PictureLink) {
                        recipe.PictureLink = "img/default.ico";
                    }
                    list +=
                        '<div class="span4" data-recipe-id="' + recipe.Id + '">' +
                            '<h3><a href="#" id="btn-recipe">' + $("<div> /").html(recipe.Title).text() + '</a></h3>' +
                            '<img src="' + $("<div />").html(recipe.PictureLink).text() + '" class="img-polaroid" />' +
                        '</div>';

                    number++;
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
//		    '<form class="form-search">' +
                  '<div>' +
                      '<input type="text" id = "input-field" class="input-medium search-query" />' +
                      '<button  class="btn" id="btn-search">Search</button>' +
                  '</div>' +
  //          '</form>' +
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

    function buildRecipeDetailsUI(recipe) {
        var html =''+            
            '<h2>' + recipe.Title + '</h2>' +
            '<div class="row">' +
            '<div class="span2">' +
            '<div><img src="' + recipe.PictureLink + '" class="img-circle" /></div>' +
            '<div><br />' +
            '<button class="btn btn-primary" id="btn-submit-like">Like</button>' +
            '</div>'+
            '</div>' +
        '<div class="span8">'+
        '<div class="tabbable tabs-right"> ' +
        '<ul class="nav nav-tabs"> ';
        for (var i = 0; i < recipe.PreparationSteps.length; i++) {
            if (i === 0) {
                html += ' <li class="active"><a href="#' + (i+1) + '" data-toggle="tab">Step '+ (i+1) +'</a></li> ';
            }
            else {
                html += ' <li class=""><a href="#' + (i+1) + '" data-toggle="tab">Step ' + (i+1) + '</a></li> ';
            }
        }
        html +=
        '  </ul> ' +
        '  <div class="tab-content"> ';
        for (var j = 0; j < recipe.PreparationSteps.length; j++) {
            if (!recipe.PreparationSteps[j].PictureLink) {
                recipe.PreparationSteps[j].PictureLink = "img/default.ico";
            }
            if (j === 0) {
                
                html +=
                '    <div class="tab-pane active" id="' + (j+1) + '"> ' +
                '      <h4>Step ' + (j+1) + '&nbsp; Description </h4>' +
                '<p>' + recipe.PreparationSteps[j].Description + '&nbsp; Time: '+ recipe.PreparationSteps[j].PreparationTime +'min. </p>' +
                '    <p><img src="' + recipe.PreparationSteps[j].PictureLink + '" class="img-rounded picSteps" /></p></div> ';
            }
            else {
                html +=
                '    <div class="tab-pane" id="' + (j+1) + '"> ' +
                '      <h4>Step ' + (j+1) + '&nbsp; Description</h4> ' +
                '<p>' + recipe.PreparationSteps[j].Description + '&nbsp; Time: ' + recipe.PreparationSteps[j].PreparationTime + 'min. </p>' +
                '    <p><img src="' + recipe.PreparationSteps[j].PictureLink + '" class="img-rounded picSteps" /></p></div> ';
            }
        }
        html +=
         '  </div>' +
         '</div> ' +
                '<h4>Comments: </h4>' +
                    '<div>';
                for (var p = 0; p < recipe.Comments.length; p++) {
                    html +=
                        '<p><h5>' + recipe.Comments[p].AuthorName + ':</h5> &nbsp; ' + recipe.Comments[p].Content + '</p>';
                }
                html += '</div>' +
                '<label for="new-comment"><h4>Add Comment:</h4></label>' +
                '<textarea id="new-comment" rows="10" class="span6"></textarea>' +
                '<br />' +
                '<button class="btn btn-primary" id="btn-submit-comment">Submit comment</button>' +
                '</div>' +
         '</div>';

        return html;
    }

    return {
        registerForm:buildRegisterForm,
        loginForm:buildLoginForm,
        mainUI:buildMainUI,
        recipesList:buildRecipesList,
        userOperationUI: buildUserOperationUI,
        createRecipe: buildCreateRecipeUI,
        recipeDetailsUI:buildRecipeDetailsUI
    }

}());