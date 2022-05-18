<template>
    <div class="content-container">
        <h2>
            Home page
        </h2>
        <ul class="bots-list">
            <li class="bot-element" v-for="bot in allBots" :key="bot.id">
                <BotShort :bot="bot"/>
            </li>
            <li id="create-bot-btn" v-on:click="routeCreatePage">
            </li>
        </ul>
        <div class="bot-info">
        </div>        
    </div>
</template>

<script>
import BotShort from '../components/bot/BotShort.vue'
import { getAllBots } from '../services/clients/bots.service.client'
import { routePaths } from '../common/constants'

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
        },
        routeCreatePage() {            
            this.$router.push({ name: routePaths.editor.name, params: { id: 'create' }});
        }
    },
    mounted () {
        this.initBotsList();
    }
}
</script>

<style lang="scss" scoped>
@import '@/styles/colours';
@import '@/styles/fonts';

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

#create-bot-btn:hover {
    height: 50px;
    background-color: #71588E;
    cursor: pointer;
}

#create-bot-btn:active {
    height: 50px;
    background-color: white;
    cursor: pointer;
}

#create-script-btn {
    height: 50px;
}

#create-script-btn:hover {
    height: 50px;
    background-color: #71588E;
    cursor: pointer;
}

#create-script-btn:active {
    height: 50px;
    background-color: white;
    cursor: pointer;
}

</style>