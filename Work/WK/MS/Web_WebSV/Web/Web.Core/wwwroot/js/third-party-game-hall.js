var GameTabType;
(function (GameTabType) {
    GameTabType[GameTabType["All"] = 1] = "All";
    GameTabType[GameTabType["Hot"] = 2] = "Hot";
    GameTabType[GameTabType["Favorite"] = 3] = "Favorite";
})(GameTabType || (GameTabType = {}));
var ThirdPartyGameHall = (function () {
    function ThirdPartyGameHall(gameLobbyType, bodyClass) {
        var _this = this;
        this.gameLobbyType = gameLobbyType;
        this.bodyClass = bodyClass;
        this.gameTabType = GameTabType.All;
        this.searchGames = function (isAppendContent, lastNo) {
            var url = $.toTokenPath(globalVariables.GetUrl("GameLobby/GetGameLobbyGameList"));
            var searchGameName = $.trim(_this.$searchName.val());
            var filterNos = [];
            var $jqGameList = $(".jqGameList");
            var self = _this;
            if (_this.gameTabType === GameTabType.Favorite) {
                filterNos = _this.favoriteCollection;
            }
            if (!isAppendContent) {
                $jqGameList.empty();
            }
            $.ajax2({
                url: url,
                type: "POST",
                data: {
                    gameLobbyTypeValue: _this.gameLobbyType,
                    gameTabType: _this.gameTabType,
                    searchGameName: searchGameName,
                    lastNo: lastNo,
                    filterNos: filterNos
                },
                success: function (response) {
                    var $response = $(response);
                    if (isAppendContent && $response.find(".jqNoResult").length > 0) {
                        return;
                    }
                    $jqGameList.append($response);
                    self.initFavoriteButton();
                }
            });
        };
        $(function () {
            var isAppendContent = false;
            $("body").addClass(_this.bodyClass);
            _this.$search = $("#SearchNameButton");
            _this.$searchName = $("#SearchGameName");
            _this.$searchName.keypress(function (e) {
                if (e.which === 13) {
                    _this.searchGames(isAppendContent);
                }
            });
            _this.$search.click(function () { return _this.searchGames(isAppendContent); });
            _this.initFavoriteButton();
            _this.searchGames(isAppendContent);
            var self = _this;
            window.addEventListener('scroll', function () {
                if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
                    var appendContent = true;
                    var lastNo = $(".jqGamePanel:last").data("id");
                    self.searchGames(appendContent, lastNo);
                }
            });
        });
    }
    Object.defineProperty(ThirdPartyGameHall.prototype, "favoriteCollection", {
        get: function () {
            return $.localStorage.get("".concat(this.gameLobbyType, "FavoriteCollection")) || [];
        },
        set: function (value) {
            $.localStorage.set("".concat(this.gameLobbyType, "FavoriteCollection"), value);
        },
        enumerable: false,
        configurable: true
    });
    ThirdPartyGameHall.prototype.switchGameTabType = function (type) {
        var isAppendContent = false;
        this.gameTabType = type;
        $(event.currentTarget).addClass("active").siblings().removeClass("active");
        this.$searchName.val("");
        this.searchGames(isAppendContent);
    };
    ThirdPartyGameHall.prototype.gotoGame = function (gameCode, isSelfOpenPage) {
        var url = $.toTokenPath(globalVariables.GetUrl("GameLobby/LoginGameLobbyGame"));
        var data = {
            GameLobbyTypeValue: this.gameLobbyType,
            MobileGameCode: gameCode
        };
        $.ajax2({
            url: url,
            type: "POST",
            data: data,
            success: function (result) {
                if (!result.IsSuccess) {
                    alert(result.Message);
                    return;
                }
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
        });
    };
    ThirdPartyGameHall.prototype.clickFavorite = function (gameId) {
        var collection = this.favoriteCollection;
        var $target = $(event.currentTarget);
        var index = collection.findIndex(function (id) { return id === gameId; });
        if (index > -1) {
            $target.removeClass("active");
            collection.splice(index, 1);
            if (this.gameTabType === GameTabType.Favorite) {
                $target.closest("li").remove();
            }
        }
        else {
            $target.addClass("active");
            collection.push(gameId);
        }
        this.favoriteCollection = collection;
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
    return ThirdPartyGameHall;
}());
