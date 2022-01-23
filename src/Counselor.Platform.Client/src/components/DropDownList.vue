<template>
    <label :for="`${name}_id`" v-if="label && label !== ''">{{ label }}</label>
    <select :id="`${name}_id`">
        <option disabled v-if="name && name !== ''">{{ name }}</option>
        <option v-for="o in options" :value="o.value" :key="o.id">{{ o.data ? o.data : o.value }}</option>
    </select>    
</template>

<script>
export default {
    name: 'DropDownList',
    props: {
        name: String,
        label: String,
        options: Array
    }
}
</script>

<style lang="scss" scoped>

</style>