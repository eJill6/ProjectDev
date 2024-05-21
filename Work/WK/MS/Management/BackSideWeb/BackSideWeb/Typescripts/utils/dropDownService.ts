class dropDownService {
    private $dropDown: JQuery<HTMLElement>;    

    constructor(selector: string) {

        this.$dropDown = $(selector);
    }

    triggerChange() {
        let value: string = this.getSelectedValue();
        this.setSelectedValue(value);
    }

    updateMenu(response: string): number {
        let $menus = $(response).find(".jqDropDownMenu a");
        this.$dropDown.next(".jqDropDownMenu").empty().append($menus);
        this.$dropDown.data("value", "").find("span").text("");
        this.$dropDown.find("input").val("");

        return $menus.length;
    }

    clearMenu(): void {
        this.$dropDown.next(".jqDropDownMenu").empty();
        this.$dropDown.data("value", "").find("span").text("");
        this.$dropDown.find("input").val("");
    }

    getSelectedValue(): string {
        return this.$dropDown.data("value");
    }

    setSelectedIndex(index: number): void {
        this.$dropDown.next(".jqDropDownMenu").children(`a:eq(${index})`).click();
    }

    setSelectedValue(value: string): boolean {
        let $link = this.$dropDown.next(".jqDropDownMenu").children(`a[data_value='${value}']`);

        if ($link.length == 0) {
            return false;
        }

        $link.click();

        return true;
    }

}