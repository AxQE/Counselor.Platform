<template>
    <div class="bot-short">
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
import { botState } from '../../common/constants'
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
    width: 10px;
    height: 10px;
    background-color: red;    
}

</style>