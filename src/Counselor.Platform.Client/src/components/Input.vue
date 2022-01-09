<template>
  <div class="input-container">
    <label
      v-if="label"
      for="name"
      class="input-label"
    >
      {{label}}
    </label>
    <input
      :type="type"
      :value="modelValue"
      v-on:input="updateValue($event.target.value)"
      :name="name"
      :id="name"
      :placeholder="placeholder"
      class="input-field"
    />
    <p
      v-if="description"
      class="input-description"
    >
    {{ description }}
    </p>
  </div>
</template>

<script>
export default {
  name: 'Input',  
  props: {
    modelValue: [String, Number],
    label: String,
    name: String,
    placeholder: String,
    description: String,
    type: {
      default: 'text',
      type: String,
    }
  },
  methods: {
    updateValue(value) {
      this.$emit('update:modelValue', value);
    },
  },
};
</script>