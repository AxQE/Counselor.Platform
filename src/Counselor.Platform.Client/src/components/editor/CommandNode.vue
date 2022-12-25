<template>
    <div ref="el" class="command-node">
        <div class="command-node-body">
            <Input 
                type="text"
                class=""
                placeholder="name"
            />
            <Input 
                type="text"
                class=""
                placeholder="response"
            />
            <Instruction />
            <Switch :isChecked="isActive" />
        </div>
    </div>    
</template>

<script>
import Input from '../Input.vue'
import Instruction from './Instruction.vue'
import Switch from '../Switch.vue'
import { defineComponent, onMounted, getCurrentInstance, readonly, ref, nextTick } from 'vue'

export default defineComponent({
    name: 'CommandNode',
    props: {        
        title: String
    },
    components: {
        Input,
        Instruction,
        Switch
    },
    data() {
        return {
            commandId: '',
            friendlyName: '',
            isActive: true
        }
    },
    setup(){
        const el = ref(null);
        const nodeId = ref(0);
        

        const updateSelect = (value) => {
            console.log(value);
        }
        
        onMounted(async () => {
            await nextTick()
            // nodeId.value = el.value.parentElement.parentElement.id.slice(5)
            // dataNode.value = df.getNodeFromId(nodeId.value)
            
            // url.value = dataNode.value.data.url;
            // method.value = dataNode.value.data.method;
        });
        
        return {
            el, updateSelect
        }
    }
})
</script>

<style lang="scss" scoped>
* {
    box-sizing: border-box;
}

.switch {
    height: 15px;
    width: 30px;
}

.slider {
    border-radius: 5px;
}

:deep(.slider:before) {
    height: 15px;
    width: 15px;
    left: 0px;
    bottom: 0px;
    border-radius: 50%;
    background-color: white;
}

:deep(input:checked + .slider:before) {
  -webkit-transform: translateX(16px);
  -ms-transform: translateX(16px);
  transform: translateX(16px);
}

</style>