import api from "@/api";
import { OverviewModel} from "@/models";
import toast from "@/toast";
import { defineComponent } from "vue";


export default defineComponent({
  data() {
    return {
      info: {} as OverviewModel,
      baseIntegral: 100,
    };
  },
  methods: {
    async getOverview() {
      try {
        this.info = await api.getOverview();
      } catch (e) {
        toast(e);
      }
    },
  },
  async created() {
    await this.getOverview();
  },
  computed: {
    memberInfo() {
      return this.info;
    },
    userIntegral(): number {
      return this.info.integral < 0 ? 0 : this.info.integral;
    },
    topIntegral(): number {
      return (Math.floor( this.userIntegral / this.baseIntegral) + 1) * this.baseIntegral;
    },
    cssWidth(): string {
      return `${this.userIntegral % this.baseIntegral}%`;
    },
  },
});
