import React, { Component } from "react";
import { Form, FormGroup, Label, Input, FormFeedback } from "reactstrap";

class CreateToggleForm extends Component {
  constructor() {
    super();

    this.state = { name: "", description: "" };
  }

  render() {
    const update = value => {
      this.setState(value);
      this.props.onChange({ ...this.state, ...value });
    };

    return (
      <Form>
        <FormGroup>
          <Label for="toggleName">Name</Label>
          <Input
            type="text"
            name="name"
            id="toggleName"
            placeholder="feature name"
            value={this.state.name}
            onChange={e => update({ name: e.target.value })}
            valid
          />
          <FormFeedback>There is already a toggle with that name</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Label for="toggleDescription">Description</Label>
          <Input
            type="textarea"
            name="description"
            id="toggleDescription"
            placeholder="a longer description about what this toggle is for"
            value={this.state.description}
            onChange={e => update({ description: e.target.value })}
          />
        </FormGroup>
      </Form>
    );
  }
}

export default CreateToggleForm;
