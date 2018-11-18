import React, { Component } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";

import CreateToggleForm from "./create-toggle-form";
import CreateButton from "./create-button";

class CreateToggleDialog extends Component {
  constructor(props) {
    super(props);

    this.state = { isOpen: false, name: "", description: "" };
    this.show = this.show.bind(this);
  }

  show() {
    this.setState({ isOpen: true });
  }

  render() {
    const toggle = () => this.setState({ isOpen: !this.state.isOpen });

    const clear = () => {
      this.setState({ name: "", description: "" });
    };

    return (
      <Modal isOpen={this.state.isOpen} toggle={toggle}>
        <ModalHeader toggle={toggle}>Create a new Toggle</ModalHeader>
        <ModalBody>
          <CreateToggleForm onChange={values => this.setState(values)} />
        </ModalBody>
        <ModalFooter>
          <Button color="danger" className="mr-auto" outline onClick={clear}>
            Clear
          </Button>{" "}
          <CreateButton
            name={this.state.name}
            description={this.state.description}
            onCreate={toggle}
          />{" "}
          <Button color="secondary" outline onClick={toggle}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
    );
  }
}

export default CreateToggleDialog;
