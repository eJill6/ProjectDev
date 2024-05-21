enum GameTabType {
    All = 1,
    Hot = 2,
    Favorite = 3
}

class ThirdPartyGameHall {
    private $searchName: JQuery<HTMLElement>;
    private $search: JQuery<HTMLElement>;
    private gameTabType: GameTabType = GameTabType.All;

    constructor(private thirdPartyCode: string, private bodyClass: string) {
        $(() => {
            $("body").addClass(this.bodyClass);
            this.$search = $("#SearchNameButton");
            this.$searchName = $("#SearchGameName");
            this.$searchName.keypress(e => {
                if (e.which === 13) {
                    this.searchGames();
                }
            });

            this.$search.click(() => this.searchGames());
            this.initFavoriteButton();
            this.getGameList();
        });
    }

    get favoriteCollection() {
        return $.localStorage.get(`${this.thirdPartyCode}FavoriteCollection`) || [];
    }

    set favoriteCollection(value: any[]) {
        $.localStorage.set(`${this.thirdPartyCode}FavoriteCollection`, value);
    }

    switchGameTabType(type: GameTabType) {
        this.gameTabType = type;
        $(event.currentTarget).addClass("active").siblings().removeClass("active");
        this.$searchName.val("");
        this.searchGames();
    }

    searchGames = (pageNumber: number = 1) => {
        let url = $.toTokenPath(globalVariables.GetUrl(`GameLobby/Get${this.thirdPartyCode}GameList?`));
        const gameName = $.trim(this.$searchName.val() as string);
        url = this.gameTabType === GameTabType.Favorite ? url + $.param({ gameIds: this.favoriteCollection }, true) : url;

        $.ajaxHtmlUpdate($("#games"), {
            url: url,
            type: "GET",
            data: { gameTabType: this.gameTabType, gameName, pageNumber }
        });
    }

    gotoGame(gameCode: string, isSelfOpenPage: boolean) {
        const url = $.toTokenPath(globalVariables.GetUrl(`GameLobby/${this.thirdPartyCode}Game?gameCode=${gameCode}`));

        $.ajax2({
            url,
            type: "GET",
            success: function (result) {
                if (result.IsSuccess) {
                    if (isSelfOpenPage == true) {
                        location.href = result.DataModel;
                    }
                    else {
                        let fullLayerParam: IFullLayerParam = new FullLayerParam();
                        fullLayerParam.url = result.DataModel;
                        fullLayerParam.isTitleVisible = false;
                        $.openFullLayer(fullLayerParam);
                        $(".jqBackLink").show();
                    }
                } else {
                    alert(result.Message);
                }
            }
        });
    }

    clickFavorite(gameId: number) {
        const collection = this.favoriteCollection;
        const $target = $(event.currentTarget);
        const index = collection.findIndex(id => id === gameId);
        if (index > -1) {
            $target.removeClass("active");
            collection.splice(index, 1);
        } else {
            $target.addClass("active");
            collection.push(gameId);
        }

        this.favoriteCollection = collection;

        if (this.gameTabType === GameTabType.Favorite) {
            this.searchGames();
        }
    }

    initFavoriteButton() {
        const $games = $(".pt_game_list_wrap>ul>li>div");
        $games.each((_, game) => {
            var $game = $(game);
            if (this.favoriteCollection.includes($game.data("id"))) {
                $game.find(".btn_favorite").addClass("active");
            }
        });
    }

    getGameList() {
        let url = $.toTokenPath(globalVariables.GetUrl(`GameLobby/Get${this.thirdPartyCode}GameList?`));
        let $jqGameList = $(".jqGameList");

        $.ajaxHtmlUpdate($jqGameList, {
            url: url,
            type: "GET",
        });
    }
}