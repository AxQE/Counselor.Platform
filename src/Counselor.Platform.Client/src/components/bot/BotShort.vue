<template>
    <div class="bot-short" v-on:click="onBotClick">
        <div class="bot-element name">
            <p>{{ this.bot.name }}</p>
        </div>

        <div class="bot-element state">
            <p>{{ this.bot.botState }}</p>
        </div>

        <div class="bot-element transport">
            <p>{{ this.bot.transport.name }}</p>
        </div>

        <div id="run-bot" class="bot-element" v-if="(!isRunning)">
        </div>
        <div id="stop-bot" class="bot-element" v-else>
        </div>
    </div>
</template>

<script>
import { botState, routePaths } from '../../common/constants'
export default {
    name: 'BotShort',
    props: {
        bot: Object        
    },
    computed: {
        isRunning() {
            return this.bot.botState === botState.Created
                || this.bot.botState === botState.Started
                || this.bot.botState === botState.Pending
        }
    },
    methods: {
        onBotClick() {
            this.$router.push({ name: routePaths.editor.name, params: { id: this.bot.id }});
        }
    }
}
</script>

<style lang="scss" scoped>

div {
    height: 100%;
}

.bot-short {
    height: 50px;
}

.bot-short:hover {
    cursor: pointer;
    background-color: lightblue;
}

.bot-short:active {
    background-color: red;
}

.bot-element {
    display: inline-block;    
}

#run-bot {
    width: 0; 
    height: 0; 
    border-top: 10px solid transparent;
    border-bottom: 10px solid transparent;  
    border-left: 10px solid green;
}

#stop-bot {
    width: 20px;
    height: 20px;
    background-color: red;    
}

#stop-bot:hover {
    border: 2px solid blue;
}

</style>