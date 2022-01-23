<template>
    <div class="content-container editor-page">
        <div class="bot-info">
            <BotInfo />
        </div>
        <div class="script-editor">
            <Flowchart />
        </div>
    </div>    
</template>

<script>
import Flowchart from '../components/editor/Flowchart.vue'
import BotInfo from '../components/bot/BotInfo.vue'

export default {
    name: 'Editor',
    components: {
        Flowchart,
        BotInfo
    },
    computed: {

    },
    methods: {

    },
    getters: {
        getBotId() {
            const path = window.location.path;
        }
    }
}
</script>

<style lang="scss" scoped>

</style>