<template>
    <div class="command-nodes">
        <ul>            
            <li 
                v-for="command in commands"
                :key="command"
                :data-node="command.item"
                draggable="true"
                @dragstart="drag($event)"
                class="drag-dragflow"
                >
                <CommandNodeShort :title="command.name"/>
            </li>
        </ul>
    </div>
    <div id="drawflow" @drop="drop($event)" @dragover="allowDrop($event)"></div>    
</template>

<script>
"use strict"

import CommandNode from './CommandNode.vue'
import CommandRootNode from './CommandRootNode.vue'
import CommandNodeShort from './CommandNodeShort.vue'
import Drawflow from 'drawflow'
import styleDrawflow from 'drawflow/dist/drawflow.min.css'
import flowchart from '../../assets/styles/flowchart.css'
import { onMounted, shallowRef, h, getCurrentInstance, render, readonly, ref } from 'vue'
import { getAllTransports, getTransportCommand } from '../../services/clients/transports.service.client'

export default {
    name: 'Flowchart',
    data() {
        return {
            commands: []
        }
    },
    components: {
        CommandNodeShort
    },
    setup(props, context) {

        console.log('setup flowchart')
        const commandNodesList = [];

        const editor = shallowRef({});
        const dialogVisible = ref(false);
        const dialogData = ref({});
        const Vue = { version: 3, h, render };
        const internalInstance = getCurrentInstance();
        internalInstance.appContext.app._context.config.globalProperties.$df = editor;

        function exportEditor() {
            dialogData.value = editor.value.export();
            dialogVisible.value.true;
        }

        const drag = (event) => {
            if (event.type === 'touchstart') {
                mobile_item_selec = event.target.closest(".drag-drawflow").getAttribute('data-node');
            }
            else {
                event.dataTransfer.setData("node", event.target.getAttribute('data-node'));
            }
        }

        const drop = (event) => {
            if (event.type === "touchend") {
                var parentdrawflow = document.elementFromPoint( mobile_last_move.touches[0].clientX, mobile_last_move.touches[0].clientY).closest("#drawflow");                
                if(parentdrawflow != null) {
                    addNodeToDrawFlow(mobile_item_selec, mobile_last_move.touches[0].clientX, mobile_last_move.touches[0].clientY);
                }
                mobile_item_selec = '';
            } else {
                event.preventDefault();
                var data = event.dataTransfer.getData("node");
                addNodeToDrawFlow(data, event.clientX, event.clientY);
            }
        }

        const allowDrop = (event) => {
            event.preventDefault();
        }

        let mobile_item_selec = '';
        let mobile_last_move = null;

        function positionMobile(ev) {
            mobile_last_move = ev;
        }

        function addNodeToDrawFlow(name, pos_x, pos_y) {
            pos_x = pos_x * ( editor.value.precanvas.clientWidth / (editor.value.precanvas.clientWidth * editor.value.zoom)) - (editor.value.precanvas.getBoundingClientRect().x * ( editor.value.precanvas.clientWidth / (editor.value.precanvas.clientWidth * editor.value.zoom)));
            pos_y = pos_y * ( editor.value.precanvas.clientHeight / (editor.value.precanvas.clientHeight * editor.value.zoom)) - (editor.value.precanvas.getBoundingClientRect().y * ( editor.value.precanvas.clientHeight / (editor.value.precanvas.clientHeight * editor.value.zoom)));
            
            const nodeSelected = commandNodesList.find(ele => ele.item == name);
            editor.value.addNode(name, nodeSelected.input,  nodeSelected.output, pos_x, pos_y, name, {}, name, 'vue');
        }

        function initRootNode() {

            const rootNode = 
            {
                name: 'root',
                color: '#49494970',
                item: 'CommandRootNode',
                input: 0,
                output: 1
            };            

            commandNodesList.push(rootNode);
            addNodeToDrawFlow(rootNode.item, 50, 350);
        }

        onMounted(() => {
            console.log('on mounted flowchart')
            var elements = document.getElementsByClassName('drag-drawflow');
            for (var i = 0; i < elements.length; i++) {
                elements[i].addEventListener('touchend', drop, false);
                elements[i].addEventListener('touchmove', positionMobile, false);
                elements[i].addEventListener('touchstart', drag, false );
            }
                
            const id = document.getElementById("drawflow");
            editor.value = new Drawflow(id, Vue, internalInstance.appContext.app._context);
            editor.value.start();
            
            editor.value.registerNode('CommandNode', CommandNode, {}, {});
            editor.value.registerNode('CommandRootNode', CommandRootNode, {}, {});

            initRootNode();
        });

        return {
            exportEditor, commandNodesList: commandNodesList, drag, drop, allowDrop, dialogVisible, dialogData
        }
    },
    methods: {
        currentTransportCommands() {
            const transport = this.$data.transports.filter(x => x.Name === "Telegram")
            return transport.commands;
        }
    },
    async mounted() {
        const commandNodesList = this.commandNodesList;
        const availableCommands = this.commands;

        const availableTransports = await getAllTransports(true);

        if (availableTransports.data) {
            availableTransports.data.forEach(async transport => {
                const commands = []; 
                (await getTransportCommand(transport.id)).data.forEach(x =>{
                    const command = 
                        {
                            name: x.name,
                            color: '#49494970',
                            item: 'CommandNode',
                            input: 1,
                            output: 1
                        };

                    availableCommands.push(command);
                    commandNodesList.push(command);
                })
            });                    
        }
    },
}
</script>

<style lang="scss" scoped>

ul {
    list-style-type: none;
    text-align: center;    
    padding: 0;
}

li {
    display: inline-flex;    
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #494949;
}

#drawflow {
  width: 100%;
  height: 800px;
  text-align: initial;
  background: #2b2c30;
  background-size: 20px 20px;
  background-image: radial-gradient(#494949 1px, transparent 1px);  
}

</style>