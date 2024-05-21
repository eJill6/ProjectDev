import { defineComponent } from 'vue';

export default defineComponent({
    data() {
        return {
            timerId: null as unknown as number,
            randomDrawNumbers: [] as string[]
        };
    },
    methods: {
        startRandom() {
            this.stopRandom();
            this.timerId = setInterval(() => {
                let source = this.$_gameTypeDrawNumbers.slice();
                let random = source.sort(() => 0.5 - Math.random());
                let result = random.slice(0, this.$_gameTypeDrawNumberCount);

                this.randomDrawNumbers = result;
            }, 50);
        },
        stopRandom() {
            clearInterval(this.timerId);
        }
    },
    created() {
        this.startRandom();
    },
    beforeUnmount() {
        this.stopRandom();
    },
    computed: {
        $_gameTypeDrawNumbers(): string[] {
            throw new Error('$_gameTypeDrawNumbers:Override this method by option.');
        },
        $_gameTypeDrawNumberCount(): number {
            throw new Error('$_gameTypeDrawNumberCount:Override this method by option.');
        }
    }
});