<template>
    <div class="command-node">
        <div class="command-node-header">
        </div>
        <div class="command-node-body">
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

.command-node {
    width: 100px;
    height: 300px;
}

.command-node-header {
    height: 50px;
    width: 100%;
}
</style>