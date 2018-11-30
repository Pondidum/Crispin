import React from "react";
import { Popover, PopoverBody, PopoverHeader, Input } from "reactstrap";
import Glyph from "../../../util/glyph";

class TagAddDialog extends React.Component {
  constructor(props) {
    super(props);

    this.state = { open: false };
    this.editor = React.createRef();
  }

  render() {
    const toggle = () => {
      this.setState({ open: !this.state.open });
    };

    const accept = e => {
      e.preventDefault();
      this.setState({ open: false });
      const value = this.editor.current.value;

      if (value && value.trim() !== "") {
        this.props.addTag(value);
      }
    };

    const handleKeyDown = e => {
      if (e.key == "Enter") {
        return accept(e);
      }
      if (e.key == "Escape") {
        return toggle();
      }
    };

    return (
      <span>
        <a href="#" onClick={toggle} id="AddToggleLink">
          <Glyph name="plus" alt="Add Tag" />
        </a>
        <Popover
          placement="right"
          target="AddToggleLink"
          isOpen={this.state.open}
          toggle={toggle}
        >
          <PopoverHeader>Add Tag</PopoverHeader>
          <PopoverBody>
            <Input
              autoFocus
              type="text"
              onKeyDown={handleKeyDown}
              innerRef={this.editor}
            />
            <small className="d-inline float-right">
              <a href="#" onClick={toggle}>
                cancel
              </a>{" "}
              <a href="#" onClick={accept}>
                ok
              </a>
            </small>
          </PopoverBody>
        </Popover>
      </span>
    );
  }
}

export default TagAddDialog;
