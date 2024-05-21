<template>
    <div class="flex_height">
      <div class="overflow no_scrollbar">
        <div class="empty_state_content">
          <div class="empty_state">
            <div class="icon">
              <img src="@/assets/images/public/pic_empty.png" alt="" />
            </div>
            <div class="nodata">{{ messageString }}</div>
          </div>
        </div>
      </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
    props:{
        message:String
    },
    computed:{
        messageString(){
            return this.message || "";
        }
    }
})
</script>