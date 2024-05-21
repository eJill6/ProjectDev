<template>
    <div class="popup-cover " style="position: absolute; right: 0; bottom: 0; top: 0; left: 0">
        <div class="position-fixed w-100 rounded-main bottom-0 bg-pop" >
            <div class="position-relative ">
                <div class="d-flex justify-content-center align-items-center  text-black fw-bold betlist_title_text">请输入自定义底分</div>
                <div class="position-absolute list_closebtn" @click="navigateToBet">
                    <div class="cusror-pointer "></div>
                </div>
            </div>
        <p class="text-black fs-2 text-center text-black pt-4">自定义范围 {{ allowedMinBaseAmount }}~{{ allowedMaxBaseAmount }}</p>
        <form class="label_input pr-5-sm pl-5-sm pr-5-sm pl-5-sm pt-5 pb-2 ">
 			<input type="number" id="fname" name="firstname" placeholder="请输入" v-model="customBaseAmount">
        </form>
        <div class="d-flex justify-content-center pt-6 pb-9">
            <button class=" second-gradient rounded-full text-white fs-6 fs-6-sm pt-4-5 pb-4-5 list_confirm_btn" @click="confirmBaseAmount">确定</button>
        </div>
    </div>
    </div>

</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaseAmount } from '@/mixins';
import { MutationType } from "@/store";
import AssetImage from '@/components/shared/AssetImage.vue';

export default defineComponent({
    components: {AssetImage},
    mixins: [BaseAmount],
    data() {
        return {
            customBaseAmount: 0
        };
    },
    methods: {
        navigateToBet() {
            this.$router.push({ name: 'Bet' });
        },
        confirmBaseAmount() {
            if (!this.isVaild) return;

            this.$store.commit(MutationType.SetBaseAmount, this.customBaseAmount);
            this.navigateToBet();
        }
    },
    created() {
        this.customBaseAmount = this.baseAmount;
    },
    computed: {
        isVaild() {
            if (isNaN(+this.customBaseAmount))
                return false;

            if (!Number.isInteger(this.customBaseAmount))
                return false;

            return (this.customBaseAmount >= this.allowedMinBaseAmount) && (this.customBaseAmount <= this.allowedMaxBaseAmount);
        }
    }
});
</script>