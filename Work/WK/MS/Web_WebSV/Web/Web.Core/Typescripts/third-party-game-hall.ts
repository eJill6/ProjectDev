enum GameTabType {
    All = 1,
    Hot = 2,
    Favorite = 3
}

class ThirdPartyGameHall {
    private $searchName: JQuery<HTMLElement>;
    private $search: JQuery<HTMLElement>;
    private gameTabType: GameTabType = GameTabType.All;

    constructor(private gameLobbyType: string, private bodyClass: string) {
        $(() => {
            const isAppendContent: boolean = false;
            $("body").addClass(this.bodyClass);
            this.$search = $("#SearchNameButton");
            this.$searchName = $("#SearchGameName");

            this.$searchName.keypress(e => {
                if (e.which === 13) {
                    this.searchGames(isAppendContent);
                }
            });

            this.$search.click(() => this.searchGames(isAppendContent));
            this.initFavoriteButton();
            this.searchGames(isAppendContent);
            let self = this;

            window.addEventListener('scroll', () => {
                if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
                    let appendContent: boolean = true;
                    let lastNo: number = $(".jqGamePanel:last").data("id");

                    self.searchGames(appendContent, lastNo);
                }
            });            

        });
    }

    get favoriteCollection() {
        return $.localStorage.get(`${this.gameLobbyType}FavoriteCollection`) || [];
    }

    set favoriteCollection(value: any[]) {
        $.localStorage.set(`${this.gameLobbyType}FavoriteCollection`, value);
    }

    switchGameTabType(type: GameTabType) {
        const isAppendContent: boolean = false;
        this.gameTabType = type;
        $(event.currentTarget).addClass("active").siblings().removeClass("active");
        this.$searchName.val("");
        this.searchGames(isAppendContent);
    }

    searchGames = (isAppendContent: boolean, lastNo?: number) => {
        let url = $.toTokenPath(globalVariables.GetUrl("GameLobby/GetGameLobbyGameList"));
        const searchGameName = $.trim(this.$searchName.val() as string);
        let filterNos = [];
        let $jqGameList = $(".jqGameList");
        let self = this;

        if (this.gameTabType === GameTabType.Favorite) {
           filterNos = this.favoriteCollection;           
        }

        if (!isAppendContent) {
            $jqGameList.empty();
        }

        $.ajax2({
            url: url,
            type: "POST",
            data: {
                gameLobbyTypeValue: this.gameLobbyType,
                gameTabType: this.gameTabType,
                searchGameName,
                lastNo,
                filterNos
            },
            success: (response) => {
                let $response = $(response);

                if (isAppendContent && $response.find(".jqNoResult").length > 0) {
                    return;
                }

                $jqGameList.append($response);
                self.initFavoriteButton();
            }
        });
    }

    gotoGame(gameCode: string, isSelfOpenPage: boolean) {
        const url = $.toTokenPath(globalVariables.GetUrl("GameLobby/LoginGameLobbyGame"));

        let data = {
            GameLobbyTypeValue: this.gameLobbyType,
            MobileGameCode: gameCode
        };

        $.ajax2({
            url,
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
                    let fullLayerParam: IFullLayerParam = new FullLayerParam();
                    fullLayerParam.url = result.DataModel;
                    fullLayerParam.isTitleVisible = false;
                    $.openFullLayer(fullLayerParam);
                    $(".jqBackLink").show();
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

            if (this.gameTabType === GameTabType.Favorite) {
                $target.closest("li").remove();
            }
        } else {
            $target.addClass("active");
            collection.push(gameId);
        }

        this.favoriteCollection = collection;
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
}