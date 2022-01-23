<template>
    <div class="command">
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