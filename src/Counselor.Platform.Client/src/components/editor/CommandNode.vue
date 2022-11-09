<template>
    <div class="flowchart-node">
        <div class="flowchart-node-header">
        </div>
        <div class="flowchart-node-body">
            <Input 
                v-model="commandId"
                type="text"
                class=""
                placeholder="name" 
            />
            <Input 
                v-model="friendlyName"
                type="text"
                class=""
                placeholder="friendly name" 
            />
            <Input 
                v-model="isActive"
                type="checkbox"
                class=""
                name="active"
            />
            <Instruction />
        </div>
    </div>    
</template>

<script>
import Input from '../Input.vue'
import Instruction from './Instruction.vue'

export default {
    name: 'Command',
    props: {

    },
    components: {
        Input,
        Instruction
    },
    data() {
        return {
            commandId: '',
            friendlyName: '',
            isActive: true
        }
    }
}
</script>

<style lang="scss" scoped>

.flowchart-node {
    width: 200px;
    height: 100px;
    border-radius: 50%;
    background-color: black;
}

.flowchart-node-header {
    height: 12px;
    width: 100%;
}

.input-container {
    width: 50px;
}

.input-container input {
    width: 150px;
}
</style>