/// <reference path="http-requester.js" />
/// <reference path="class.js" />
/// <reference path="http://crypto-js.googlecode.com/svn/tags/3.1.2/build/rollups/sha1.js" />
var persisters = (function () {
    var nickname = localStorage.getItem("nickname");
    var sessionKey = localStorage.getItem("sessionKey");
    function saveUserData(userData) {
        console.log("Recieved: " + userData);
        localStorage.setItem("nickname", userData.Nickname);
        localStorage.setItem("sessionKey", userData.SessionKey);
        nickname = userData.Nickname;
        sessionKey = userData.SessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        nickname = "";
        sessionKey = "";
    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.recipe = new RecipePersister(this.rootUrl);
            this.comment = new CommentsPersister(this.rootUrl);
            this.imageUpload = new ImageUploadPersister(this.rootUrl);
        },
        isUserLoggedIn: function () {
            var isSessionKeyAvailable = nickname != null && sessionKey != null && (typeof sessionKey !== "undefined");

            console.log(isSessionKeyAvailable);

            return isSessionKeyAvailable;
        },

        nickname: function () {
            return nickname;
        }
    });
    var UserPersister = Class.create({
        init: function (rootUrl) {
            //...api/user/
            this.rootUrl = rootUrl + "users/";
        },
        getNickname: function () {
            return nickname;
        },
        login: function (user, success, error) {
            var url = this.rootUrl + "login";
            var userData = {
                UserName: user.username,
                Password: user.password
                //authCode: CryptoJS.SHA1(user.username + user.password).toString()
            };

            httpRequester.postJSON(url, userData,
				function (data) {
				    saveUserData(data);
				    success(data);
				}, error);
        },
        register: function (user, success, error) {
            var url = this.rootUrl + "register";
            var userData = {
                UserName: user.username,
                Nickname: user.nickname,
                Password: user.password
                //authCode: CryptoJS.SHA1(user.username + user.password).toString()
            };
            httpRequester.postJSON(url, userData,
				function (data) {
				    var UserData = {
                            Nickname: user.nickname,
                            SessionKey: data
                        };
				    saveUserData(UserData);
				    success(data);
				}, error);
        },
        logout: function (success, error) {
            var url = this.rootUrl + "logout/" + sessionKey;
            httpRequester.getJSON(url, function (data) {
                clearUserData();
                success(data);
            }, error)
        },
        checkSessionKey: function (success, error) {
            var url = this.rootUrl + "checkSessionKey/" + sessionKey;
            httpRequester.getJSON(url, success, function () {
                error();
                clearUserData();
            })
        },
        test: function (success, error) {
            var url = this.rootUrl + "test";
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error)
        },
    });

    var RecipePersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "recipes/";
        },
        create: function (recipeData, success, error) {

            var url = this.rootUrl + "add/" + sessionKey;

            httpRequester.postJSON(url, recipeData, success, error);
        },
        start: function (recipeId, success, error) {
            var url = this.rootUrl + "start/" + sessionKey;
            httpRequester.postJSON(url, recipeId, success, error);
        },
        open: function (recipeId, success, error) {
            var url = this.rootUrl +"recipe/"+ recipeId;
            httpRequester.getJSON(url, success, error);
        },
        page: function (pageNumber, success, error) {
            var url = this.rootUrl + "page/" + pageNumber;
            httpRequester.getJSON(url, success, error);
        },
        search: function (queryString, success, error) {
            var url = this.rootUrl + "search/" + queryString;
            httpRequester.getJSON(url, success, error);
        },
    });

    var ImageUploadPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "users/";
        },
        uploadRawAjax: function (formatData, success, error) {

            var url = this.rootUrl + "upload";

            $.ajax({
                url: url,
                type: "POST",
                data: formatData,
                processData: false,
                contentType: false,
                success: function (response) {
                    //success(response);
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    console.log(errorMessage); // Optional
                    error;
                }
            });
        },
    });


    //Adding Comments
    var CommentsPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "comments/";
        },
        create: function (commentData, success, error) {
            var url = this.rootUrl + sessionKey;
            httpRequester.postJSON(url, commentData, success, error);
        },

    });
    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());