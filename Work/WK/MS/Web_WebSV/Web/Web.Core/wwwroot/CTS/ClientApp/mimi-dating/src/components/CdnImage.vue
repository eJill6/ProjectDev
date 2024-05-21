<template>
    <img
        :class="class"
        :alt="alt"
        :src="imgSrc"
       @click="$emit('clickEvent')"
    />
</template>

<script lang="ts">
import { defineComponent } from "vue";
import api from '@/api';

export default defineComponent({
    props: {
        class: String,
        src: {
            type: String,
            required: true,
        },
        alt: String,
    },
    emits:['clickEvent'],
    methods: {
    },
    computed: {
        imgSrc() {
            return api.getImageUrl(this.src);
        },
    }
});
</script>