<template>
    <div class="content-container">
        <h2>
            Home page
        </h2>
        <ul class="bots-list">
            <li class="bot-element" v-for="bot in allBots" :key="bot.id">
                <BotShort :bot="bot"/>
            </li>
            <li id="create-bot-btn">
            </li>
        </ul>
        <ul class="scripts-list">
        </ul>
        <div class="bot-info">
        </div>        
    </div>
</template>

<script>
import  { getAllBots } from '../services/clients/bots.service.client'
import BotShort from '../components/bot/BotShort.vue'

export default {
    name: 'Home',
    data() {
        return {
            bots: []
        }
    },    
    components: {
        BotShort        
    },
    computed: {
        allBots() {
            return this.bots.filter(x => x.transport !== undefined);
        }
    },
    methods: {
        async initBotsList() {

            this.bots = (await getAllBots()).data;
        }
    },
    mounted () {
        this.initBotsList();
    }
}
</script>

<style lang="scss" scoped>

ul {
    list-style: none;
    padding: 0;
}

li {
    border: 1px solid black;
}

#create-bot-btn {
    height: 50px;
}

</style>