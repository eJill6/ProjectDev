var GameTabType;
(function (GameTabType) {
    GameTabType[GameTabType["All"] = 1] = "All";
    GameTabType[GameTabType["Hot"] = 2] = "Hot";
    GameTabType[GameTabType["Favorite"] = 3] = "Favorite";
})(GameTabType || (GameTabType = {}));
var ThirdPartyGameHall = (function () {
    function ThirdPartyGameHall(thirdPartyCode, bodyClass) {
        var _this = this;
        this.thirdPartyCode = thirdPartyCode;
        this.bodyClass = bodyClass;
        this.gameTabType = GameTabType.All;
        this.searchGames = function (pageNumber) {
            if (pageNumber === void 0) { pageNumber = 1; }
            var url = $.toTokenPath(globalVariables.GetUrl("GameLobby/Get".concat(_this.thirdPartyCode, "GameList?")));
            var gameName = $.trim(_this.$searchName.val());
            url = _this.gameTabType === GameTabType.Favorite ? url + $.param({ gameIds: _this.favoriteCollection }, true) : url;
            $.ajaxHtmlUpdate($("#games"), {
                url: url,
                type: "GET",
                data: { gameTabType: _this.gameTabType, gameName: gameName, pageNumber: pageNumber }
            });
        };
        $(function () {
            $("body").addClass(_this.bodyClass);
            _this.$search = $("#SearchNameButton");
            _this.$searchName = $("#SearchGameName");
            _this.$searchName.keypress(function (e) {
                if (e.which === 13) {
                    _this.searchGames();
                }
            });
            _this.$search.click(function () { return _this.searchGames(); });
            _this.initFavoriteButton();
            _this.getGameList();
        });
    }
    Object.defineProperty(ThirdPartyGameHall.prototype, "favoriteCollection", {
        get: function () {
            return $.localStorage.get("".concat(this.thirdPartyCode, "FavoriteCollection")) || [];
        },
        set: function (value) {
            $.localStorage.set("".concat(this.thirdPartyCode, "FavoriteCollection"), value);
        },
        enumerable: false,
        configurable: true
    });
    ThirdPartyGameHall.prototype.switchGameTabType = function (type) {
        this.gameTabType = type;
        $(event.currentTarget).addClass("active").siblings().removeClass("active");
        this.$searchName.val("");
        this.searchGames();
    };
    ThirdPartyGameHall.prototype.gotoGame = function (gameCode, isSelfOpenPage) {
        var url = $.toTokenPath(globalVariables.GetUrl("GameLobby/".concat(this.thirdPartyCode, "Game?gameCode=").concat(gameCode)));
        $.ajax2({
            url: url,
            type: "GET",
            success: function (result) {
                if (result.IsSuccess) {
                    if (isSelfOpenPage == true) {
                        location.href = result.DataModel;
                    }
                    else {
                        var fullLayerParam = new FullLayerParam();
                        fullLayerParam.url = result.DataModel;
                        fullLayerParam.isTitleVisible = false;
                        $.openFullLayer(fullLayerParam);
                        $(".jqBackLink").show();
                    }
                }
                else {
                    alert(result.Message);
                }
            }
        });
    };
    ThirdPartyGameHall.prototype.clickFavorite = function (gameId) {
        var collection = this.favoriteCollection;
        var $target = $(event.currentTarget);
        var index = collection.findIndex(function (id) { return id === gameId; });
        if (index > -1) {
            $target.removeClass("active");
            collection.splice(index, 1);
        }
        else {
            $target.addClass("active");
            collection.push(gameId);
        }
        this.favoriteCollection = collection;
        if (this.gameTabType === GameTabType.Favorite) {
            this.searchGames();
        }
    };
    ThirdPartyGameHall.prototype.initFavoriteButton = function () {
        var _this = this;
        var $games = $(".pt_game_list_wrap>ul>li>div");
        $games.each(function (_, game) {
            var $game = $(game);
            if (_this.favoriteCollection.includes($game.data("id"))) {
                $game.find(".btn_favorite").addClass("active");
            }
        });
    };
    ThirdPartyGameHall.prototype.getGameList = function () {
        var url = $.toTokenPath(globalVariables.GetUrl("GameLobby/Get".concat(this.thirdPartyCode, "GameList?")));
        var $jqGameList = $(".jqGameList");
        $.ajaxHtmlUpdate($jqGameList, {
            url: url,
            type: "GET",
        });
    };
    return ThirdPartyGameHall;
}());
