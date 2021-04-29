extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var add_new_effect = true;
var database_loaded = false;
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start():
	if (database_loaded == false):
		var jsonDictionary = get_parent().get_parent().call("read_data", "Effect")
		for i in range(jsonDictionary.size()):
			var effect_data = jsonDictionary["effect" + String(i)]
			var show_list = effect_data["data_type"]
			var value2 = effect_data["value2"]

			$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.add_item(effect_data["name"])
			if (show_list["show"] == true):
				$EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.add_item(show_list["data"])
			else:
				$EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.add_item("disabled")
			$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.add_item(effect_data["value1"])
			if (value2["show"] == true):
				$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.add_item(value2["data"])
			else:
				$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.add_item("-1")
		database_loaded = true

func _on_AddEffect_pressed():
	add_new_effect = true;
	$AddEffect.popup_centered()

func _on_EditEffect_pressed():
	if ($EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_selected_items()[0] > -1):
		add_new_effect = false;
		var id = $EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_selected_items()[0]
		var name = $EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_item_text(id)
		var data_types = $EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.get_item_text(id)
		var value1 = int($EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.get_item_text(id))
		var value2 = int($EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.get_item_text(id))

		$AddEffect/VBoxContainer/Name/LineEdit.text = name
		if (data_types == "disabled"):
			$AddEffect/VBoxContainer/show_list/CheckButton.pressed = false
			$AddEffect/VBoxContainer/show_list/OptionButton.disabled = true
		else:
			$AddEffect/VBoxContainer/show_list/CheckButton.pressed = true
			$AddEffect/VBoxContainer/show_list/OptionButton.disabled = false
			match (data_types):
				"States":
					$AddEffect/VBoxContainer/show_list/OptionButton.select(0)
				"Stats":
					$AddEffect/VBoxContainer/show_list/OptionButton.select(1)
				"Weapon Types":
					$AddEffect/VBoxContainer/show_list/OptionButton.select(2)
				"Armor Types":
					$AddEffect/VBoxContainer/show_list/OptionButton.select(3)
				"Elements":
					$AddEffect/VBoxContainer/show_list/OptionButton.select(4)
				"Skill Types":
					$AddEffect/VBoxContainer/show_list/OptionButton.select(5)
		$AddEffect/VBoxContainer/Value1/OptionButton.select(value1)
		if (value2 == -1):
			$AddEffect/VBoxContainer/Value2/CheckButton.pressed = false
			$AddEffect/VBoxContainer/Value2/OptionButton.disabled = true
		else:
			$AddEffect/VBoxContainer/Value2/CheckButton.pressed = true
			$AddEffect/VBoxContainer/Value2/OptionButton.disabled = false
			$AddEffect/VBoxContainer/Value2/OptionButton.select(value2)
		$AddEffect.popup_centered()

func _on_RemoveEffect_pressed():
	var selected_effect = $EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_selected_items()[0];
	if (selected_effect > -1):
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.RemoveItem(selected_effect)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.RemoveItem(selected_effect)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.RemoveItem(selected_effect)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.RemoveItem(selected_effect)

func _on_clearEffects_pressed():
	$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.clear()
	$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.clear()

func _on_show_listCheckButton_pressed():
	if ($AddEffect/VBoxContainer/show_list/CheckButton.pressed == true):
		$AddEffect/VBoxContainer/show_list/OptionButton.disabled = false
	else:
		$AddEffect/VBoxContainer/show_list/OptionButton.disabled = true

func _on_Value2CheckButton_pressed():
	if ($AddEffect/VBoxContainer/Value2/CheckButton.pressed == true):
		$AddEffect/VBoxContainer/Value2/OptionButton.disabled = false
	else:
		$AddEffect/VBoxContainer/Value2/OptionButton.disabled = true

func _on_AddEffectConfirm_pressed():
	var name = $AddEffect/VBoxContainer/Name/LineEdit.text
	var selected = 0
	var data_type = "disabled"
	if ($AddEffect/VBoxContainer/show_list/CheckButton.pressed == true):
		selected = $AddEffect/VBoxContainer/show_list/OptionButton.selected
		data_type = $AddEffect/VBoxContainer/show_list/OptionButton.get_item_text(selected)
	var value1 = $AddEffect/VBoxContainer/Value1/OptionButton.selected
	var value2 = -1
	if ($AddEffect/VBoxContainer/Value2/CheckButton.pressed == true):
		value2 = $AddEffect/VBoxContainer/Value2/OptionButton.selected
	if (add_new_effect == false):
		var id = $EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_selected_items()[0];
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.SetItemtext(id, name)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.SetItemtext(id, data_type)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.SetItemtext(id, value1)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.SetItemtext(id, value2)
	else:
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.add_item(name)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.add_item(data_type)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.add_item(value1)
		$EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.add_item(value2)
	$AddEffect.hide()

func _on_AddEffectCancel_pressed():
	$AddEffect.hide()

func _on_SaveEffects_pressed():
	var size = $EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_item_count()
	var effect_list
	for i in range(size):
		var effect_data
		var show_list
		var value2
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames.get_item_text(i)
		var data_type = $EffectLabel/PanelContainer/VBoxContainer/Effects/data_types.get_item_text(i)
		if (data_type == "disabled"):
			show_list["show"] = false
			show_list["data"] = ""
		else:
			show_list["show"] = true
			show_list["data"] = data_type
		effect_data["data_type"] = show_list
		effect_data["value1"] = int($EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1.get_item_text(i))
		var value2Val = int($EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2.get_item_text(i))
		if (value2Val == -1):
			value2["show"] = false
			value2["data"] = ""
		else:
			value2["show"] = true
			value2["data"] = value2Val
		effect_data["value2"] = value2
		effect_list["effect" + String(i)] = effect_data
	get_parent().get_parent().call("store_data", "Effect", effect_list)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
